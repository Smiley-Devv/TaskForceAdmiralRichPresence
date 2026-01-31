namespace TaskForceAdmiralLiveRPC
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnReconnect = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabEvents = new System.Windows.Forms.TabPage();
            this.txtEventLog = new System.Windows.Forms.RichTextBox();
            this.panelEventStats = new System.Windows.Forms.Panel();
            this.lblCurrentAction = new System.Windows.Forms.Label();
            this.lblTotalHits = new System.Windows.Forms.Label();
            this.lblTotalKills = new System.Windows.Forms.Label();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.tabUpdates = new System.Windows.Forms.TabPage();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.lblLatestVersion = new System.Windows.Forms.Label();
            this.btnCheckUpdates = new System.Windows.Forms.Button();
            this.txtReleaseNotes = new System.Windows.Forms.RichTextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.linkGitHub = new System.Windows.Forms.LinkLabel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabEvents.SuspendLayout();
            this.panelEventStats.SuspendLayout();
            this.tabUpdates.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.btnReconnect);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(800, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(267, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "⚓ Task Force Admiral RPC";
            // 
            // btnReconnect
            // 
            this.btnReconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(101)))), ((int)(((byte)(242)))));
            this.btnReconnect.FlatAppearance.BorderSize = 0;
            this.btnReconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReconnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnReconnect.ForeColor = System.Drawing.Color.White;
            this.btnReconnect.Location = new System.Drawing.Point(670, 15);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(110, 30);
            this.btnReconnect.TabIndex = 1;
            this.btnReconnect.Text = "Reconnect RPC";
            this.btnReconnect.UseVisualStyleBackColor = false;
            this.btnReconnect.Click += new System.EventHandler(this.btnReconnect_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabEvents);
            this.tabControl.Controls.Add(this.tabUpdates);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabControl.Location = new System.Drawing.Point(0, 60);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 490);
            this.tabControl.TabIndex = 1;
            // 
            // tabEvents
            // 
            this.tabEvents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.tabEvents.Controls.Add(this.txtEventLog);
            this.tabEvents.Controls.Add(this.panelEventStats);
            this.tabEvents.Location = new System.Drawing.Point(4, 28);
            this.tabEvents.Name = "tabEvents";
            this.tabEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabEvents.Size = new System.Drawing.Size(792, 458);
            this.tabEvents.TabIndex = 0;
            this.tabEvents.Text = "📊 Battle Events";
            // 
            // txtEventLog
            // 
            this.txtEventLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.txtEventLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEventLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEventLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtEventLog.ForeColor = System.Drawing.Color.White;
            this.txtEventLog.Location = new System.Drawing.Point(3, 83);
            this.txtEventLog.Name = "txtEventLog";
            this.txtEventLog.ReadOnly = true;
            this.txtEventLog.Size = new System.Drawing.Size(786, 372);
            this.txtEventLog.TabIndex = 0;
            this.txtEventLog.Text = "Waiting for battle events...\n";
            // 
            // panelEventStats
            // 
            this.panelEventStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(43)))));
            this.panelEventStats.Controls.Add(this.lblCurrentAction);
            this.panelEventStats.Controls.Add(this.lblTotalHits);
            this.panelEventStats.Controls.Add(this.lblTotalKills);
            this.panelEventStats.Controls.Add(this.btnClearLog);
            this.panelEventStats.Controls.Add(this.chkAutoScroll);
            this.panelEventStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEventStats.Location = new System.Drawing.Point(3, 3);
            this.panelEventStats.Name = "panelEventStats";
            this.panelEventStats.Size = new System.Drawing.Size(786, 80);
            this.panelEventStats.TabIndex = 1;
            // 
            // lblCurrentAction
            // 
            this.lblCurrentAction.AutoSize = true;
            this.lblCurrentAction.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCurrentAction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(220)))), ((int)(((byte)(93)))));
            this.lblCurrentAction.Location = new System.Drawing.Point(10, 10);
            this.lblCurrentAction.Name = "lblCurrentAction";
            this.lblCurrentAction.Size = new System.Drawing.Size(224, 20);
            this.lblCurrentAction.TabIndex = 0;
            this.lblCurrentAction.Text = "🎮 Monitoring for events...";
            // 
            // lblTotalHits
            // 
            this.lblTotalHits.AutoSize = true;
            this.lblTotalHits.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalHits.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblTotalHits.Location = new System.Drawing.Point(10, 40);
            this.lblTotalHits.Name = "lblTotalHits";
            this.lblTotalHits.Size = new System.Drawing.Size(100, 15);
            this.lblTotalHits.TabIndex = 1;
            this.lblTotalHits.Text = "💥 Hits Taken: 0";
            // 
            // lblTotalKills
            // 
            this.lblTotalKills.AutoSize = true;
            this.lblTotalKills.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalKills.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblTotalKills.Location = new System.Drawing.Point(150, 40);
            this.lblTotalKills.Name = "lblTotalKills";
            this.lblTotalKills.Size = new System.Drawing.Size(130, 15);
            this.lblTotalKills.TabIndex = 2;
            this.lblTotalKills.Text = "✈️ Enemy Destroyed: 0";
            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(70)))), ((int)(((byte)(75)))));
            this.btnClearLog.FlatAppearance.BorderSize = 0;
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnClearLog.ForeColor = System.Drawing.Color.White;
            this.btnClearLog.Location = new System.Drawing.Point(680, 10);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(90, 25);
            this.btnClearLog.TabIndex = 3;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Checked = true;
            this.chkAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoScroll.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkAutoScroll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.chkAutoScroll.Location = new System.Drawing.Point(680, 45);
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Size = new System.Drawing.Size(82, 17);
            this.chkAutoScroll.TabIndex = 4;
            this.chkAutoScroll.Text = "Auto-scroll";
            this.chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // tabUpdates
            // 
            this.tabUpdates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.tabUpdates.Controls.Add(this.lblCurrentVersion);
            this.tabUpdates.Controls.Add(this.lblLatestVersion);
            this.tabUpdates.Controls.Add(this.btnCheckUpdates);
            this.tabUpdates.Controls.Add(this.txtReleaseNotes);
            this.tabUpdates.Controls.Add(this.btnDownload);
            this.tabUpdates.Controls.Add(this.linkGitHub);
            this.tabUpdates.Location = new System.Drawing.Point(4, 28);
            this.tabUpdates.Name = "tabUpdates";
            this.tabUpdates.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdates.Size = new System.Drawing.Size(792, 458);
            this.tabUpdates.TabIndex = 1;
            this.tabUpdates.Text = "🔄 Updates";
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCurrentVersion.ForeColor = System.Drawing.Color.White;
            this.lblCurrentVersion.Location = new System.Drawing.Point(20, 20);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(160, 19);
            this.lblCurrentVersion.TabIndex = 0;
            this.lblCurrentVersion.Text = "Current Version: v1.0.0";
            // 
            // lblLatestVersion
            // 
            this.lblLatestVersion.AutoSize = true;
            this.lblLatestVersion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLatestVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblLatestVersion.Location = new System.Drawing.Point(20, 50);
            this.lblLatestVersion.Name = "lblLatestVersion";
            this.lblLatestVersion.Size = new System.Drawing.Size(200, 19);
            this.lblLatestVersion.TabIndex = 1;
            this.lblLatestVersion.Text = "Latest Version: Checking...";
            // 
            // btnCheckUpdates
            // 
            this.btnCheckUpdates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(101)))), ((int)(((byte)(242)))));
            this.btnCheckUpdates.FlatAppearance.BorderSize = 0;
            this.btnCheckUpdates.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckUpdates.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCheckUpdates.ForeColor = System.Drawing.Color.White;
            this.btnCheckUpdates.Location = new System.Drawing.Point(630, 20);
            this.btnCheckUpdates.Name = "btnCheckUpdates";
            this.btnCheckUpdates.Size = new System.Drawing.Size(140, 35);
            this.btnCheckUpdates.TabIndex = 2;
            this.btnCheckUpdates.Text = "Check for Updates";
            this.btnCheckUpdates.UseVisualStyleBackColor = false;
            this.btnCheckUpdates.Click += new System.EventHandler(this.btnCheckUpdates_Click);
            // 
            // txtReleaseNotes
            // 
            this.txtReleaseNotes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.txtReleaseNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReleaseNotes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtReleaseNotes.ForeColor = System.Drawing.Color.White;
            this.txtReleaseNotes.Location = new System.Drawing.Point(20, 90);
            this.txtReleaseNotes.Name = "txtReleaseNotes";
            this.txtReleaseNotes.ReadOnly = true;
            this.txtReleaseNotes.Size = new System.Drawing.Size(750, 320);
            this.txtReleaseNotes.TabIndex = 3;
            this.txtReleaseNotes.Text = "Release notes will appear here when checking for updates.";
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(181)))), ((int)(((byte)(129)))));
            this.btnDownload.Enabled = false;
            this.btnDownload.FlatAppearance.BorderSize = 0;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnDownload.ForeColor = System.Drawing.Color.White;
            this.btnDownload.Location = new System.Drawing.Point(630, 420);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(140, 30);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "Download Update";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // linkGitHub
            // 
            this.linkGitHub.AutoSize = true;
            this.linkGitHub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linkGitHub.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(101)))), ((int)(((byte)(242)))));
            this.linkGitHub.Location = new System.Drawing.Point(20, 427);
            this.linkGitHub.Name = "linkGitHub";
            this.linkGitHub.Size = new System.Drawing.Size(91, 15);
            this.linkGitHub.TabIndex = 5;
            this.linkGitHub.TabStop = true;
            this.linkGitHub.Text = "View on GitHub";
            this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGitHub_LinkClicked);
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.panelFooter.Controls.Add(this.lblStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 550);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(800, 30);
            this.panelFooter.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(242)))), ((int)(((byte)(135)))));
            this.lblStatus.Location = new System.Drawing.Point(15, 7);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(68, 15);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Initializing...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(800, 580);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Task Force Admiral Live RPC";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabEvents.ResumeLayout(false);
            this.panelEventStats.ResumeLayout(false);
            this.panelEventStats.PerformLayout();
            this.tabUpdates.ResumeLayout(false);
            this.tabUpdates.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabEvents;
        private System.Windows.Forms.RichTextBox txtEventLog;
        private System.Windows.Forms.Panel panelEventStats;
        private System.Windows.Forms.Label lblCurrentAction;
        private System.Windows.Forms.Label lblTotalHits;
        private System.Windows.Forms.Label lblTotalKills;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.CheckBox chkAutoScroll;
        private System.Windows.Forms.TabPage tabUpdates;
        private System.Windows.Forms.Label lblCurrentVersion;
        private System.Windows.Forms.Label lblLatestVersion;
        private System.Windows.Forms.Button btnCheckUpdates;
        private System.Windows.Forms.RichTextBox txtReleaseNotes;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.LinkLabel linkGitHub;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label lblStatus;
    }
}
