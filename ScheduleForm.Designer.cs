namespace wildcat_one_windows
{
    partial class ScheduleForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            var contentBg = Color.FromArgb(240, 242, 245);
            const int formWidth = 900;
            const int formHeight = 680;

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(formWidth, formHeight);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "My Class Schedule";
            Font = new Font("Segoe UI", 9F);
            BackColor = contentBg;

            // --- Schedule Header Panel ---
            scheduleHeaderPanel = new Panel();
            scheduleHeaderPanel.Location = new Point(20, 12);
            scheduleHeaderPanel.Size = new Size(860, 70);
            scheduleHeaderPanel.BackColor = Color.White;

            scheduleTitle = new Label();
            scheduleTitle.Text = "My Class Schedule";
            scheduleTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            scheduleTitle.ForeColor = Color.FromArgb(122, 26, 61);
            scheduleTitle.Location = new Point(18, 8);
            scheduleTitle.AutoSize = true;

            semesterComboBox = new ComboBox();
            semesterComboBox.Font = new Font("Segoe UI", 9.5F);
            semesterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            semesterComboBox.Location = new Point(540, 12);
            semesterComboBox.Size = new Size(300, 26);

            semesterInfoLabel = new Label();
            semesterInfoLabel.Text = "";
            semesterInfoLabel.Font = new Font("Segoe UI", 9F);
            semesterInfoLabel.ForeColor = Color.FromArgb(100, 100, 100);
            semesterInfoLabel.Location = new Point(18, 44);
            semesterInfoLabel.AutoSize = true;

            scheduleHeaderPanel.Controls.Add(scheduleTitle);
            scheduleHeaderPanel.Controls.Add(semesterComboBox);
            scheduleHeaderPanel.Controls.Add(semesterInfoLabel);

            // --- Schedule Grid Container (scrollable) ---
            scheduleGridContainer = new Panel();
            scheduleGridContainer.Location = new Point(20, 92);
            scheduleGridContainer.Size = new Size(860, formHeight - 112);
            scheduleGridContainer.AutoScroll = true;
            scheduleGridContainer.BackColor = Color.White;

            // --- Schedule Grid Panel (custom painted, DoubleBuffered) ---
            // 80px time labels + 7 * 108px day columns = 836px wide
            // 36px header + 27 * 40px slots = 1116px tall
            scheduleGridPanel = new DoubleBufferedPanel();
            scheduleGridPanel.Location = new Point(0, 0);
            scheduleGridPanel.Size = new Size(836, 1116);
            scheduleGridPanel.BackColor = Color.White;

            scheduleGridContainer.Controls.Add(scheduleGridPanel);

            Controls.Add(scheduleHeaderPanel);
            Controls.Add(scheduleGridContainer);
        }

        #endregion

        private Panel scheduleHeaderPanel;
        private Label scheduleTitle;
        private ComboBox semesterComboBox;
        private Label semesterInfoLabel;
        private Panel scheduleGridContainer;
        private DoubleBufferedPanel scheduleGridPanel;
    }
}
