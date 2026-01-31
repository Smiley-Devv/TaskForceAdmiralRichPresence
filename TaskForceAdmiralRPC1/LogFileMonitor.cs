using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace TaskForceAdmiralLiveRPC
{
    public class LogFileMonitor : IDisposable
    {
        private FileSystemWatcher watcher;
        private string logFilePath;
        private long lastReadPosition = 0;
        private FileStream fs;
        private StreamReader reader;
        private System.Threading.Timer pollTimer;
        private readonly object lockObj = new object();

        public event Action<BattleEvent> BattleEventDetected;
        public event Action<string> StatusChanged;
        public event Action<string> ErrorOccurred;

        public LogFileMonitor()
        {
            FindLogFile();
        }

        private void FindLogFile()
        {
            try
            {
                // Possible Steam locations (x86 & x64)
                string[] possiblePaths = {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                 "Steam", "steamapps", "common", "Task Force Admiral", "bin", "master", "Plankton.log"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                                 "Steam", "steamapps", "common", "Task Force Admiral", "bin", "master", "Plankton.log"),

                    // App base / bin
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Plankton.log"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plankton.log"),

                    // My Documents
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                 "My Games", "TaskForceAdmiral", "logs", "Plankton.log"),

                    // Current directory / bin
                    Path.Combine(Directory.GetCurrentDirectory(), "bin", "Plankton.log")
                };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        logFilePath = path;
                        StatusChanged?.Invoke($"Found log file: {path}");
                        InitializeMonitoring();
                        return;
                    }
                }

                StatusChanged?.Invoke("Log file not found - will watch for creation");
                WatchForLogCreation();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"Error finding log: {ex.Message}");
            }
        }

        private void WatchForLogCreation()
        {
            string watchPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            if (!Directory.Exists(watchPath))
                watchPath = AppDomain.CurrentDomain.BaseDirectory;

            watcher = new FileSystemWatcher(watchPath, "*.log");
            watcher.Created += (s, e) =>
            {
                if (e.Name.Contains("Plankton"))
                {
                    logFilePath = e.FullPath;
                    StatusChanged?.Invoke($"Log file created: {logFilePath}");
                    InitializeMonitoring();
                }
            };
            watcher.EnableRaisingEvents = true;
        }

        private void InitializeMonitoring()
        {
            try
            {
                if (string.IsNullOrEmpty(logFilePath) || !File.Exists(logFilePath))
                    return;

                // Open FileStream once for the life of the monitor
                fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new StreamReader(fs);

                // Start at end of file
                lastReadPosition = fs.Length;
                fs.Seek(lastReadPosition, SeekOrigin.Begin);

                // Start timer
                pollTimer = new System.Threading.Timer(_ => CheckForNewContent(), null, 0, 500);

                StatusChanged?.Invoke("Log monitoring started");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"Failed to initialize monitoring: {ex.Message}");
            }
        }

        private void CheckForNewContent()
        {
            lock (lockObj) // Prevent re-entrant calls
            {
                try
                {
                    if (fs == null || reader == null || !File.Exists(logFilePath))
                        return;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            try
                            {
                                ParseLogLine(line);
                            }
                            catch (Exception exLine)
                            {
                                ErrorOccurred?.Invoke($"Parse line error: {exLine.Message}");
                            }
                        }
                    }

                    lastReadPosition = fs.Position;
                }
                catch (IOException ioEx)
                {
                    // File temporarily locked by game
                    StatusChanged?.Invoke($"File busy, retrying: {ioEx.Message}");
                }
                catch (Exception ex)
                {
                    ErrorOccurred?.Invoke($"Log read error: {ex.Message}");
                }
            }
        }

        private void ParseLogLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            var battleEvent = new BattleEvent { RawLine = line, Timestamp = DateTime.Now };
            bool triggerEvent = false; // Only trigger if it’s a meaningful event

            // Attack over/report
            if (line.Contains("Enemy air attack over") || line.Contains("action report"))
            {
                battleEvent.EventType = BattleEventType.AttackReport;
                triggerEvent = true;
            }
            // Aircraft destroyed
            else if (Regex.Match(line, @"-(\w+\d*\w*)\s+destroyed").Success)
            {
                var m = Regex.Match(line, @"-(\w+\d*\w*)\s+destroyed");
                battleEvent.EventType = BattleEventType.AircraftDestroyed;
                battleEvent.AircraftType = m.Groups[1].Value;
                triggerEvent = true;
            }
            // Ship hit by bomb
            else if (Regex.Match(line, @"-([\w\s\-()]+)\s+hit by a bomb").Success)
            {
                var m = Regex.Match(line, @"-([\w\s\-()]+)\s+hit by a bomb");
                battleEvent.EventType = BattleEventType.ShipHitBomb;
                battleEvent.ShipName = m.Groups[1].Value.Trim();
                triggerEvent = true;
            }
            // Ship hit by torpedo
            else if (Regex.Match(line, @"-([\w\s\-()]+)\s+hit by a torpedo").Success)
            {
                var m = Regex.Match(line, @"-([\w\s\-()]+)\s+hit by a torpedo");
                battleEvent.EventType = BattleEventType.ShipHitTorpedo;
                battleEvent.ShipName = m.Groups[1].Value.Trim();
                triggerEvent = true;
            }
            // Radio chatter
            else if (line.Contains("This is") || line.Contains("Target in sight") || line.Contains("bogeys") || line.Contains("bandits"))
            {
                battleEvent.EventType = BattleEventType.RadioChatter;
                triggerEvent = true;
            }
            // Ammo report
            else if (line.Contains("Ammo expended"))
            {
                battleEvent.EventType = BattleEventType.AmmoReport;
                triggerEvent = true;
            }
            // Date marker
            else if (Regex.Match(line, @"(\w+\s+\d+\s+\d{4})").Success)
            {
                var m = Regex.Match(line, @"(\w+\s+\d+\s+\d{4})");
                battleEvent.EventType = BattleEventType.DateMarker;
                battleEvent.GameDate = m.Groups[1].Value;
                triggerEvent = true;
            }

            // Only invoke the event if we marked it as important
            if (triggerEvent)
            {
                battleEvent.Message = line.Trim();
                BattleEventDetected?.Invoke(battleEvent);
            }
        }

        public void Dispose()
        {
            pollTimer?.Dispose();
            reader?.Dispose();
            fs?.Dispose();
            watcher?.Dispose();
        }
    }

    public enum BattleEventType
    {
        Generic,
        AttackReport,
        AircraftDestroyed,
        ShipHitBomb,
        ShipHitTorpedo,
        RadioChatter,
        AmmoReport,
        DateMarker
    }

    public class BattleEvent
    {
        public BattleEventType EventType { get; set; }
        public string Message { get; set; }
        public string RawLine { get; set; }
        public DateTime Timestamp { get; set; }
        public string ShipName { get; set; }
        public string AircraftType { get; set; }
        public string GameDate { get; set; }
    }
}
