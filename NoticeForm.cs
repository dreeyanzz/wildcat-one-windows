namespace wildcat_one_windows
{
    public class NoticeForm : Form
    {
        public NoticeForm()
        {
            InitializeNoticeForm();
        }

        private void InitializeNoticeForm()
        {
            // === Form ===
            Text = "Wildcat One";
            ClientSize = new Size(560, 580);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(248, 245, 240);
            Font = new Font("Segoe UI", 9F);

            var maroon = Color.FromArgb(122, 26, 61);
            var darkText = Color.FromArgb(52, 73, 94);

            // === Card Panel ===
            var card = new Panel
            {
                Size = new Size(500, 520),
                Location = new Point(30, 28),
                BackColor = Color.White,
            };

            // Top accent bar
            card.Paint += (s, e) =>
            {
                using var brush = new SolidBrush(maroon);
                e.Graphics.FillRectangle(brush, 0, 0, card.Width, 5);
            };

            // === Title ===
            var titleLabel = new Label
            {
                Text = "Before You Continue",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = maroon,
                Location = new Point(30, 20),
                AutoSize = true,
            };

            // === Intro text ===
            var introLabel = new Label
            {
                Text =
                    "Wildcat One is not an official CIT-U application. It is an independent, "
                    + "student-made wrapper that provides a faster and more optimized interface "
                    + "for the existing CIT-U student portal.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = darkText,
                Location = new Point(30, 56),
                Size = new Size(440, 50),
                AutoSize = false,
            };

            // === Bullet points ===
            var points = new (string title, string detail)[]
            {
                (
                    "Your credentials are safe.",
                    "Your student ID and password are sent directly to CIT-U's official servers — this app never stores, logs, or intercepts them."
                ),
                (
                    "No data collection.",
                    "This app does not collect, track, or send your personal information to any third-party server. Zero analytics, zero tracking."
                ),
                (
                    "Same data, better experience.",
                    "Everything you see here — your grades, schedule, professors — comes directly from CIT-U's systems. This app is just a different way to view it."
                ),
                (
                    "Why does this exist?",
                    "The official portal can be slow and isn't as optimal. This wrapper was built to give fellow students a smoother experience."
                ),
            };

            var yPos = 116;
            foreach (var (title, detail) in points)
            {
                // Bullet dot
                var bullet = new Label
                {
                    Text = "●",
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = maroon,
                    Location = new Point(32, yPos + 2),
                    AutoSize = true,
                };
                card.Controls.Add(bullet);

                // Bold title
                var titleLbl = new Label
                {
                    Text = title,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = darkText,
                    Location = new Point(50, yPos),
                    AutoSize = true,
                };
                card.Controls.Add(titleLbl);

                // Detail text
                var detailLbl = new Label
                {
                    Text = detail,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Location = new Point(50, yPos + 20),
                    Size = new Size(420, 46),
                    AutoSize = false,
                };
                card.Controls.Add(detailLbl);

                yPos += 80;
            }

            // === "I Understand" Button ===
            var acknowledgeButton = new Button
            {
                Text = "I Understand, Continue",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = maroon,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(440, 44),
                Location = new Point(30, 448),
                Cursor = Cursors.Hand,
            };
            acknowledgeButton.FlatAppearance.BorderSize = 0;
            acknowledgeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(142, 46, 81);
            acknowledgeButton.Click += (s, e) =>
            {
                DialogResult = DialogResult.OK;
                Close();
            };

            card.Controls.Add(titleLabel);
            card.Controls.Add(introLabel);
            card.Controls.Add(acknowledgeButton);

            Controls.Add(card);
        }
    }
}
