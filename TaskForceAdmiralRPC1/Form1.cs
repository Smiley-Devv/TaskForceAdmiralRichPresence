using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using DiscordRPC;
using DiscordRPC.Logging;

namespace TaskForceAdmiralLiveRPC
{
    public partial class Form1 : Form
    {
        private LogFileMonitor logMonitor;
        private DiscordRpcClient client;
        private GitHubUpdateChecker updateChecker;
        private bool rpcConnected = false;
        private string rpcStateFile;

        // Event statistics
        private int totalHits = 0;
        private int totalKills = 0;
        private string lastAction = "";

        public Form1()
        {
            InitializeComponent();

            // Set up RPC state file path
            string appData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TaskForceAdmiralLiveRPC"
            );
            Directory.CreateDirectory(appData);
            rpcStateFile = Path.Combine(appData, "rpc.json");

            InitializeDiscordRPC();
            InitializeLogMonitor();
            InitializeUpdateChecker();

            // Show startup dialog after form is shown
            this.Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            // Show modern notification dialog
            var result = ModernDialog.Show(
                "Welcome to TFA RPC",
                "Would you like to load your last battle session stats from rpc.json?"
            );

            if (result == DialogResult.Yes)
            {
                LoadLastSession();
            }

            // Check for updates after dialog
            CheckForUpdatesAsync();
        }

        private void LoadLastSession()
        {
            try
            {
                if (File.Exists(rpcStateFile))
                {
                    string json = File.ReadAllText(rpcStateFile);
                    var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("TotalHits", out JsonElement hitsElem))
                        totalHits = hitsElem.GetInt32();

                    if (root.TryGetProperty("TotalKills", out JsonElement killsElem))
                        totalKills = killsElem.GetInt32();

                    if (root.TryGetProperty("LastAction", out JsonElement actionElem))
                        lastAction = actionElem.GetString() ?? "";

                    // Update UI
                    lblTotalHits.Text = $"💥 Hits Taken: {totalHits}";
                    lblTotalKills.Text = $"✈️ Enemy Destroyed: {totalKills}";
                    lblCurrentAction.Text = $"🎮 {lastAction}";

                    AppendColoredText($"Loaded previous session: {totalHits} hits, {totalKills} kills\n",
                        Color.FromArgb(87, 242, 135));

                    UpdateStatusLabel("Previous session loaded", Color.FromArgb(87, 242, 135));
                }
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"Failed to load session: {ex.Message}", Color.FromArgb(237, 66, 69));
            }
        }

        private void InitializeDiscordRPC()
        {
            try
            {
                client = new DiscordRpcClient("1466871575247720583");
                client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

                client.OnReady += (sender, e) =>
                {
                    rpcConnected = true;
                    UpdateStatusLabel("Discord RPC Connected", Color.FromArgb(87, 242, 135));
                };

                client.OnConnectionFailed += (sender, e) =>
                {
                    rpcConnected = false;
                    UpdateStatusLabel("Discord RPC Failed", Color.FromArgb(237, 66, 69));
                };

                client.OnError += (sender, e) =>
                {
                    UpdateStatusLabel($"RPC Error: {e.Message}", Color.FromArgb(237, 66, 69));
                };

                client.Initialize();
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"RPC Init Error: {ex.Message}", Color.FromArgb(237, 66, 69));
            }
        }

        private void InitializeLogMonitor()
        {
            try
            {
                logMonitor = new LogFileMonitor();
                logMonitor.BattleEventDetected += LogMonitor_BattleEventDetected;
                logMonitor.StatusChanged += (msg) => UpdateStatusLabel(msg, Color.FromArgb(255, 220, 93));
                logMonitor.ErrorOccurred += (err) => UpdateStatusLabel(err, Color.FromArgb(237, 66, 69));

                UpdateStatusLabel("Monitoring Plankton.log for battle events...", Color.FromArgb(87, 242, 135));
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"Log Monitor Error: {ex.Message}", Color.FromArgb(237, 66, 69));
            }
        }

        private void InitializeUpdateChecker()
        {
            updateChecker = new GitHubUpdateChecker();
            updateChecker.UpdateAvailable += UpdateChecker_UpdateAvailable;
            updateChecker.StatusChanged += (msg) =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((Action)(() => lblLatestVersion.Text = msg));
                }
                else
                {
                    lblLatestVersion.Text = msg;
                }
            };
        }

        private async void CheckForUpdatesAsync()
        {
            await updateChecker.CheckForUpdates();
        }

        private void LogMonitor_BattleEventDetected(BattleEvent evt)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => LogMonitor_BattleEventDetected(evt)));
                return;
            }

            // Update statistics
            switch (evt.EventType)
            {
                case BattleEventType.ShipHitBomb:
                case BattleEventType.ShipHitTorpedo:
                    totalHits++;
                    lblTotalHits.Text = $"💥 Hits Taken: {totalHits}";
                    lastAction = evt.Message;
                    break;

                case BattleEventType.AircraftDestroyed:
                    totalKills++;
                    lblTotalKills.Text = $"✈️ Enemy Destroyed: {totalKills}";
                    lastAction = evt.Message;
                    break;

                case BattleEventType.RadioChatter:
                    lastAction = evt.Message;
                    break;

                case BattleEventType.AttackReport:
                    lastAction = "Attack concluded - generating report";
                    break;
            }

            // Update current action label
            lblCurrentAction.Text = $"🎮 {lastAction}";

            // Add to event log with color coding
            Color eventColor = GetEventColor(evt.EventType);
            string timestamp = evt.Timestamp.ToString("HH:mm:ss");
            string icon = GetEventIcon(evt.EventType);

            AppendColoredText($"[{timestamp}] {icon} {evt.Message}\n", eventColor);

            // Auto-scroll if enabled
            if (chkAutoScroll.Checked)
            {
                txtEventLog.SelectionStart = txtEventLog.Text.Length;
                txtEventLog.ScrollToCaret();
            }

            // Update Discord RPC
            UpdateDiscordRPC(evt);

            // Save state
            SaveRpcState(evt);
        }

        private void AppendColoredText(string text, Color color)
        {
            txtEventLog.SelectionStart = txtEventLog.TextLength;
            txtEventLog.SelectionLength = 0;
            txtEventLog.SelectionColor = color;
            txtEventLog.AppendText(text);
            txtEventLog.SelectionColor = txtEventLog.ForeColor;
        }

        private Color GetEventColor(BattleEventType eventType)
        {
            switch (eventType)
            {
                case BattleEventType.ShipHitBomb:
                case BattleEventType.ShipHitTorpedo:
                    return Color.FromArgb(237, 66, 69); // Red
                case BattleEventType.AircraftDestroyed:
                    return Color.FromArgb(87, 242, 135); // Green
                case BattleEventType.RadioChatter:
                    return Color.FromArgb(255, 220, 93); // Yellow
                case BattleEventType.AttackReport:
                    return Color.FromArgb(88, 101, 242); // Blue
                default:
                    return Color.FromArgb(200, 200, 200); // Gray
            }
        }

        private string GetEventIcon(BattleEventType eventType)
        {
            switch (eventType)
            {
                case BattleEventType.ShipHitBomb:
                    return "💥";
                case BattleEventType.ShipHitTorpedo:
                    return "🌊";
                case BattleEventType.AircraftDestroyed:
                    return "✈️";
                case BattleEventType.RadioChatter:
                    return "📻";
                case BattleEventType.AttackReport:
                    return "📊";
                case BattleEventType.DateMarker:
                    return "📅";
                default:
                    return "📝";
            }
        }

        private void UpdateDiscordRPC(BattleEvent evt)
        {
            if (!rpcConnected || client == null)
                return;

            try
            {
                string details = "In Battle";
                string state = lastAction;

                // Customize based on event type
                if (evt.EventType == BattleEventType.ShipHitBomb || evt.EventType == BattleEventType.ShipHitTorpedo)
                {
                    details = $"🚨 {evt.ShipName} under attack!";
                    state = $"💥 {totalHits} hits taken | ✈️ {totalKills} kills";
                }
                else if (evt.EventType == BattleEventType.RadioChatter)
                {
                    details = "Radio: " + evt.Message.Substring(0, Math.Min(128, evt.Message.Length));
                    state = $"💥 {totalHits} hits | ✈️ {totalKills} kills";
                }
                else if (evt.EventType == BattleEventType.AttackReport)
                {
                    details = "Battle Report Generated";
                    state = $"Session: {totalHits} hits taken, {totalKills} enemy destroyed";
                }

                client.SetPresence(new RichPresence()
                {
                    Details = details,
                    State = state,
                    Timestamps = Timestamps.Now,
                    Assets = new Assets()
                    {
                        LargeImageKey = "tfa_icon",
                        LargeImageText = "Task Force Admiral"
                    }
                });
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"RPC Update Failed: {ex.Message}", Color.FromArgb(237, 66, 69));
            }
        }

        private void SaveRpcState(BattleEvent evt)
        {
            try
            {
                var state = new
                {
                    WhenUpdated = DateTime.Now,
                    LastEvent = evt.Message,
                    EventType = evt.EventType.ToString(),
                    TotalHits = totalHits,
                    TotalKills = totalKills,
                    LastAction = lastAction,
                    ShipName = evt.ShipName ?? "",
                    AircraftType = evt.AircraftType ?? "",
                    GameDate = evt.GameDate ?? ""
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(state, options);
                File.WriteAllText(rpcStateFile, json);
            }
            catch { /* Silent fail */ }
        }

        private void UpdateChecker_UpdateAvailable(UpdateInfo info)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => UpdateChecker_UpdateAvailable(info)));
                return;
            }

            lblLatestVersion.Text = $"Latest Version: v{info.LatestVersion} (New!)";
            lblLatestVersion.ForeColor = Color.FromArgb(87, 242, 135);

            txtReleaseNotes.Text = $"Version {info.LatestVersion} - {info.PublishedDate:MMMM d, yyyy}\n\n";
            txtReleaseNotes.AppendText(info.ReleaseNotes);

            btnDownload.Enabled = true;
            btnDownload.Tag = info.DownloadUrl;
        }

        private void UpdateStatusLabel(string text, Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => UpdateStatusLabel(text, color)));
                return;
            }

            lblStatus.Text = text;
            lblStatus.ForeColor = color;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            logMonitor?.Dispose();
            client?.Dispose();
            base.OnFormClosing(e);
        }

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.Dispose();
            }
            InitializeDiscordRPC();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtEventLog.Clear();
            txtEventLog.AppendText("Event log cleared.\n");
            totalHits = 0;
            totalKills = 0;
            lblTotalHits.Text = "💥 Hits Taken: 0";
            lblTotalKills.Text = "✈️ Enemy Destroyed: 0";
        }

        private async void btnCheckUpdates_Click(object sender, EventArgs e)
        {
            btnCheckUpdates.Enabled = false;
            btnCheckUpdates.Text = "Checking...";

            await updateChecker.CheckForUpdates();

            btnCheckUpdates.Enabled = true;
            btnCheckUpdates.Text = "Check for Updates";
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (btnDownload.Tag != null)
            {
                string url = btnDownload.Tag.ToString();
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open browser: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void linkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://github.com/Smiley-Devv/TaskForceAdmiralRichPresence";
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open browser: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
