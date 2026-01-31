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
        private System.Threading.Timer pollTimer;

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
                // Common locations for the log file
                string[] possiblePaths = {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Plankton.log"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plankton.log"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "TaskForceAdmiral", "logs", "Plankton.log"),
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

                // If not found, watch common directories
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
            {
                watchPath = AppDomain.CurrentDomain.BaseDirectory;
            }

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
            if (string.IsNullOrEmpty(logFilePath) || !File.Exists(logFilePath))
                return;

            // Start at end of file
            using (var fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                lastReadPosition = fs.Length;
            }

            // Poll every 500ms for new content
            pollTimer = new System.Threading.Timer(_ => CheckForNewContent(), null, 0, 500);

            StatusChanged?.Invoke("Log monitoring started");
        }

        private void CheckForNewContent()
        {
            try
            {
                if (!File.Exists(logFilePath))
                    return;

                using (var fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (fs.Length <= lastReadPosition)
                        return; // No new content

                    fs.Seek(lastReadPosition, SeekOrigin.Begin);
                    using (var reader = new StreamReader(fs))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            ParseLogLine(line);
                        }
                    }

                    lastReadPosition = fs.Position;
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"Log read error: {ex.Message}");
            }
        }

        private void ParseLogLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            var battleEvent = new BattleEvent { RawLine = line, Timestamp = DateTime.Now };

            // Detect event type and parse accordingly

            // Attack over/report
            if (line.Contains("Enemy air attack over") || line.Contains("action report"))
            {
                battleEvent.EventType = BattleEventType.AttackReport;
                battleEvent.Message = line;
                BattleEventDetected?.Invoke(battleEvent);
                return;
            }

            // Aircraft destroyed
            var destroyedMatch = Regex.Match(line, @"-(\w+\d*\w*)\s+destroyed");
            if (destroyedMatch.Success)
            {
                battleEvent.EventType = BattleEventType.AircraftDestroyed;
                battleEvent.AircraftType = destroyedMatch.Groups[1].Value;
                battleEvent.Message = $"{battleEvent.AircraftType} shot down";
                BattleEventDetected?.Invoke(battleEvent);
                return;
            }

            // Ship hit by bomb
            var bombMatch = Regex.Match(line, @"-([\w\s\-()]+)\s+hit by a bomb");
            if (bombMatch.Success)
            {
                battleEvent.EventType = BattleEventType.ShipHitBomb;
                battleEvent.ShipName = bombMatch.Groups[1].Value.Trim();
                battleEvent.Message = $"{battleEvent.ShipName} hit by bomb!";
                BattleEventDetected?.Invoke(battleEvent);
                return;
            }

            // Ship hit by torpedo
            var torpedoMatch = Regex.Match(line, @"-([\w\s\-()]+)\s+hit by a torpedo");
            if (torpedoMatch.Success)
            {
                battleEvent.EventType = BattleEventType.ShipHitTorpedo;
                battleEvent.ShipName = torpedoMatch.Groups[1].Value.Trim();
                battleEvent.Message = $"{battleEvent.ShipName} hit by torpedo!";
                BattleEventDetected?.Invoke(battleEvent);
                return;
            }

            // Radio chatter (pilot communications)
            if (line.Contains("This is") || line.Contains("Target in sight") ||
                line.Contains("bogeys") || line.Contains("bandits"))
            {
                battleEvent.EventType = BattleEventType.RadioChatter;
                battleEvent.Message = line.Trim();
                BattleEventDetected?.Invoke(battleEvent);
                return;
            }

            // Ammo expended
            if (line.Contains("Ammo expended"))
            {
                battleEvent.EventType = BattleEventType.AmmoReport;
                battleEvent.Message = line.Trim();
                BattleEventDetected?.Invoke(battleEvent);
                return;
            }

            // Date markers
            var dateMatch = Regex.Match(line, @"(\w+\s+\d+\s+\d{4})");
            if (dateMatch.Success)
            {
                battleEvent.EventType = BattleEventType.DateMarker;
                battleEvent.GameDate = dateMatch.Groups[1].Value;
                battleEvent.Message = $"Date: {battleEvent.GameDate}";
                BattleEventDetected?.Invoke(battleEvent);
                return;
            }

            // Generic log entry
            if (!string.IsNullOrWhiteSpace(line))
            {
                battleEvent.EventType = BattleEventType.Generic;
                battleEvent.Message = line.Trim();
                BattleEventDetected?.Invoke(battleEvent);
            }
        }

        public void Dispose()
        {
            pollTimer?.Dispose();
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