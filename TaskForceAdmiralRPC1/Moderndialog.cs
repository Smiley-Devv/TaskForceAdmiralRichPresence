using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskForceAdmiralLiveRPC
{
    public class ModernDialog : Form
    {
        private Panel panelTop;
        private Label lblTitle;
        private Label lblMessage;
        private Button btnYes;
        private Button btnNo;
        private Label lblIcon;

        public ModernDialog(string title, string message)
        {
            InitializeComponents(title, message);
        }

        private void InitializeComponents(string title, string message)
        {
            // Form settings
            this.Text = title;
            this.Size = new Size(500, 240);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(32, 34, 37);
            this.DoubleBuffered = true;

            // Add shadow effect
            this.Padding = new Padding(1);

            // Top panel with icon and title
            panelTop = new Panel
            {
                BackColor = Color.FromArgb(47, 49, 54),
                Dock = DockStyle.Top,
                Height = 60
            };

            lblIcon = new Label
            {
                Text = "⚓",
                Font = new Font("Segoe UI", 24F),
                ForeColor = Color.FromArgb(88, 101, 242),
                Location = new Point(20, 12),
                AutoSize = true
            };

            lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(70, 18),
                AutoSize = true
            };

            panelTop.Controls.Add(lblIcon);
            panelTop.Controls.Add(lblTitle);

            // Message
            lblMessage = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(220, 221, 222),
                Location = new Point(20, 80),
                Size = new Size(460, 80),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Yes button
            btnYes = new Button
            {
                Text = "Yes, Load Last Session",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(88, 101, 242),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(270, 180),
                Size = new Size(210, 35),
                Cursor = Cursors.Hand
            };
            btnYes.FlatAppearance.BorderSize = 0;
            btnYes.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };
            btnYes.MouseEnter += (s, e) => btnYes.BackColor = Color.FromArgb(71, 82, 196);
            btnYes.MouseLeave += (s, e) => btnYes.BackColor = Color.FromArgb(88, 101, 242);

            // No button
            btnNo = new Button
            {
                Text = "No, Start Fresh",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(200, 200, 200),
                BackColor = Color.FromArgb(47, 49, 54),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 180),
                Size = new Size(210, 35),
                Cursor = Cursors.Hand
            };
            btnNo.FlatAppearance.BorderSize = 0;
            btnNo.Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };
            btnNo.MouseEnter += (s, e) => btnNo.BackColor = Color.FromArgb(57, 59, 64);
            btnNo.MouseLeave += (s, e) => btnNo.BackColor = Color.FromArgb(47, 49, 54);

            // Add border
            this.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                    Color.FromArgb(88, 101, 242), ButtonBorderStyle.Solid);
            };

            // Add controls
            this.Controls.Add(panelTop);
            this.Controls.Add(lblMessage);
            this.Controls.Add(btnYes);
            this.Controls.Add(btnNo);
        }

        public static DialogResult Show(string title, string message)
        {
            using (var dialog = new ModernDialog(title, message))
            {
                return dialog.ShowDialog();
            }
        }
    }
}