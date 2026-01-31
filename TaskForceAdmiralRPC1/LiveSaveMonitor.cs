using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TaskForceAdmiralLiveRPC
{
    public class LiveSaveMonitor : IDisposable
    {
        private FileSystemWatcher watcher;
        private string saveFolder;
        private string lastProcessedFile = "";
        private SaveSnapshot lastSnapshot;
        private System.Threading.Timer debounceTimer;
        private readonly object lockObject = new object();

        public event Action<SaveSnapshot> SnapshotUpdated;
        public event Action<string> ErrorOccurred;

        public LiveSaveMonitor()
        {
            try
            {
                string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFolder = Path.Combine(documents, "My Games", "TaskForceAdmiral", "saves");

                if (!Directory.Exists(saveFolder))
                {
                    // Try to create the directory if it doesn't exist
                    Directory.CreateDirectory(saveFolder);
                    ErrorOccurred?.Invoke($"Created save folder at: {saveFolder}");
                }

                watcher = new FileSystemWatcher(saveFolder, "*.save");
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime;
                watcher.Changed += OnFileChanged;
                watcher.Created += OnFileChanged;
                watcher.Renamed += OnFileChanged;
                watcher.EnableRaisingEvents = true;

                // Process initial save file
                ProcessLatestSave();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"Initialization error: {ex.Message}");
            }
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            // Debounce file changes (wait 500ms before processing)
            lock (lockObject)
            {
                debounceTimer?.Dispose();
                debounceTimer = new System.Threading.Timer(_ => ProcessLatestSave(), null, 500, Timeout.Infinite);
            }
        }

        private void ProcessLatestSave()
        {
            try
            {
                if (!Directory.Exists(saveFolder))
                {
                    ErrorOccurred?.Invoke("Save folder no longer exists");
                    return;
                }

                var latestSave = new DirectoryInfo(saveFolder)
                    .GetFiles("*.save")
                    .OrderByDescending(f => f.LastWriteTime)
                    .FirstOrDefault();

                if (latestSave == null)
                {
                    ErrorOccurred?.Invoke("No save files found");
                    return;
                }

                // Check if file is still being written to
                if (IsFileLocked(latestSave))
                {
                    Thread.Sleep(100);
                    return;
                }

                if (latestSave.FullName == lastProcessedFile)
                {
                    // Check if file has been modified
                    var fileInfo = new FileInfo(lastProcessedFile);
                    if (fileInfo.LastWriteTime == latestSave.LastWriteTime)
                        return;
                }

                string jsonText = File.ReadAllText(latestSave.FullName);
                using JsonDocument doc = JsonDocument.Parse(jsonText);

                var snapshot = ParseSave(doc.RootElement);

                if (lastSnapshot == null || !snapshot.Equals(lastSnapshot))
                {
                    lastSnapshot = snapshot;
                    lastProcessedFile = latestSave.FullName;
                    SnapshotUpdated?.Invoke(snapshot);
                }
            }
            catch (JsonException ex)
            {
                ErrorOccurred?.Invoke($"JSON parsing error: {ex.Message}");
            }
            catch (IOException ex)
            {
                ErrorOccurred?.Invoke($"File access error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"Error processing save: {ex.Message}");
            }
        }

        private bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }

        private SaveSnapshot ParseSave(JsonElement root)
        {
            int friendly = 0, enemy = 0, tracks = 0, attacking = 0;
            string lastDate = "", scenario = "Unknown Scenario";

            if (root.TryGetProperty("scenarioName", out JsonElement s))
                scenario = s.GetString() ?? "Unknown Scenario";

            if (root.TryGetProperty("airTrackInfos", out JsonElement airTracks))
            {
                foreach (var t in airTracks.EnumerateArray())
                {
                    int side = 0, state = 0, count = 0;

                    if (t.TryGetProperty("track", out JsonElement track))
                    {
                        if (track.TryGetProperty("side", out JsonElement se))
                            side = se.GetInt32();
                        if (track.TryGetProperty("state", out JsonElement st))
                            state = st.GetInt32();
                    }

                    if (t.TryGetProperty("history", out JsonElement history))
                    {
                        count = history.EnumerateArray().Sum(h =>
                        {
                            if (h.TryGetProperty("count", out JsonElement c))
                                return c.GetInt32();
                            return 0;
                        });

                        var last = history.EnumerateArray().LastOrDefault();
                        if (last.ValueKind != JsonValueKind.Undefined &&
                            last.TryGetProperty("date", out JsonElement dateElem))
                            lastDate = dateElem.GetString() ?? "";
                    }

                    if (side != 0)
                    {
                        tracks++;
                        enemy += count;
                        if (state == 2) attacking++;
                    }
                    else
                    {
                        friendly += count;
                    }
                }
            }

            return new SaveSnapshot
            {
                ScenarioName = scenario,
                FriendlyAircraft = friendly,
                EnemyAircraft = enemy,
                EnemyTracks = tracks,
                AttackingGroups = attacking,
                LastUpdate = lastDate
            };
        }

        public void Dispose()
        {
            debounceTimer?.Dispose();
            watcher?.Dispose();
        }
    }
}