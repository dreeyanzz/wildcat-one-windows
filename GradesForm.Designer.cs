namespace wildcat_one_windows
{
    partial class GradesForm
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
            var maroon = Color.FromArgb(122, 26, 61);
            var darkText = Color.FromArgb(52, 73, 94);
            var contentBg = Color.FromArgb(240, 242, 245);
            const int formWidth = 900;
            const int formHeight = 680;

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(formWidth, formHeight);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Academic Grades";
            Font = new Font("Segoe UI", 9F);
            BackColor = contentBg;

            // ===========================================
            // TabControl (root control)
            // ===========================================
            gradesTabControl = new TabControl();
            gradesTabControl.Dock = DockStyle.Fill;
            gradesTabControl.Font = new Font("Segoe UI", 9.5F);

            // ===========================================
            // Tab 1 — Grades
            // ===========================================
            gradesTab = new TabPage("Grades");
            gradesTab.BackColor = contentBg;
            gradesTab.Padding = new Padding(0);

            // --- Grades Header Panel ---
            gradesHeaderPanel = new Panel();
            gradesHeaderPanel.Location = new Point(20, 12);
            gradesHeaderPanel.Size = new Size(840, 70);
            gradesHeaderPanel.BackColor = Color.White;

            gradesTitle = new Label();
            gradesTitle.Text = "Academic Grades";
            gradesTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            gradesTitle.ForeColor = maroon;
            gradesTitle.Location = new Point(18, 8);
            gradesTitle.AutoSize = true;

            gradesSemesterComboBox = new ComboBox();
            gradesSemesterComboBox.Font = new Font("Segoe UI", 9.5F);
            gradesSemesterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            gradesSemesterComboBox.Location = new Point(540, 12);
            gradesSemesterComboBox.Size = new Size(280, 26);

            gradesSemesterInfoLabel = new Label();
            gradesSemesterInfoLabel.Text = "";
            gradesSemesterInfoLabel.Font = new Font("Segoe UI", 9F);
            gradesSemesterInfoLabel.ForeColor = Color.FromArgb(100, 100, 100);
            gradesSemesterInfoLabel.Location = new Point(18, 44);
            gradesSemesterInfoLabel.AutoSize = true;

            gradesHeaderPanel.Controls.Add(gradesTitle);
            gradesHeaderPanel.Controls.Add(gradesSemesterComboBox);
            gradesHeaderPanel.Controls.Add(gradesSemesterInfoLabel);

            // --- Grades Table Container (scrollable) ---
            gradesTableContainer = new Panel();
            gradesTableContainer.Location = new Point(20, 92);
            gradesTableContainer.Size = new Size(840, formHeight - 160);
            gradesTableContainer.AutoScroll = true;
            gradesTableContainer.BackColor = Color.White;

            // --- Grades DataGridView ---
            gradesDataGridView = new DataGridView();
            gradesDataGridView.Dock = DockStyle.Fill;
            gradesDataGridView.ReadOnly = true;
            gradesDataGridView.AllowUserToAddRows = false;
            gradesDataGridView.AllowUserToDeleteRows = false;
            gradesDataGridView.AllowUserToResizeRows = false;
            gradesDataGridView.RowHeadersVisible = false;
            gradesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gradesDataGridView.MultiSelect = false;
            gradesDataGridView.BorderStyle = BorderStyle.None;
            gradesDataGridView.BackgroundColor = Color.White;
            gradesDataGridView.GridColor = Color.FromArgb(230, 230, 230);
            gradesDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gradesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            gradesDataGridView.RowTemplate.Height = 50;
            gradesDataGridView.Font = new Font("Segoe UI", 9F);

            gradesDataGridView.EnableHeadersVisualStyles = false;
            gradesDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = maroon,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = Color.FromArgb(248, 249, 250),
                SelectionForeColor = maroon
            };
            gradesDataGridView.ColumnHeadersHeight = 40;
            gradesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gradesDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            gradesDataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = darkText,
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = darkText,
                Padding = new Padding(8, 0, 0, 0),
                WrapMode = DataGridViewTriState.True
            };
            gradesDataGridView.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 250, 252),
                ForeColor = darkText,
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = darkText,
                Padding = new Padding(8, 0, 0, 0),
                WrapMode = DataGridViewTriState.True
            };

            var colCourseCode = new DataGridViewTextBoxColumn { Name = "CourseCode", HeaderText = "Course Code", Width = 100 };
            var colCourseTitle = new DataGridViewTextBoxColumn { Name = "CourseTitle", HeaderText = "Course Title", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill };
            var colUnits = new DataGridViewTextBoxColumn
            {
                Name = "Units",
                HeaderText = "Units",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            var colMidterm = new DataGridViewTextBoxColumn
            {
                Name = "Midterm",
                HeaderText = "Midterm",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            var colFinal = new DataGridViewTextBoxColumn
            {
                Name = "Final",
                HeaderText = "Final",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            var colStatus = new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            gradesDataGridView.Columns.AddRange(colCourseCode, colCourseTitle, colUnits, colMidterm, colFinal, colStatus);
            gradesTableContainer.Controls.Add(gradesDataGridView);

            // --- Grades empty state label ---
            gradesEmptyLabel = new Label();
            gradesEmptyLabel.Text = "No grades available";
            gradesEmptyLabel.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            gradesEmptyLabel.ForeColor = Color.FromArgb(150, 150, 150);
            gradesEmptyLabel.AutoSize = false;
            gradesEmptyLabel.Size = new Size(840, 40);
            gradesEmptyLabel.Location = new Point(20, 200);
            gradesEmptyLabel.TextAlign = ContentAlignment.MiddleCenter;
            gradesEmptyLabel.Visible = false;

            gradesTab.Controls.Add(gradesHeaderPanel);
            gradesTab.Controls.Add(gradesTableContainer);
            gradesTab.Controls.Add(gradesEmptyLabel);

            // ===========================================
            // Tab 2 — Summary
            // ===========================================
            summaryTab = new TabPage("Summary");
            summaryTab.BackColor = contentBg;
            summaryTab.Padding = new Padding(0);

            summaryPanel = new Panel();
            summaryPanel.Location = new Point(20, 20);
            summaryPanel.Size = new Size(840, 300);
            summaryPanel.BackColor = Color.White;

            summaryTitleLabel = new Label();
            summaryTitleLabel.Text = "Grade Summary";
            summaryTitleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            summaryTitleLabel.ForeColor = maroon;
            summaryTitleLabel.Location = new Point(18, 16);
            summaryTitleLabel.AutoSize = true;

            summaryGwaLabel = new Label();
            summaryGwaLabel.Text = "GWA: —";
            summaryGwaLabel.Font = new Font("Segoe UI", 12F);
            summaryGwaLabel.ForeColor = darkText;
            summaryGwaLabel.Location = new Point(18, 60);
            summaryGwaLabel.AutoSize = true;

            summarySemesterLabel = new Label();
            summarySemesterLabel.Text = "Semester: —";
            summarySemesterLabel.Font = new Font("Segoe UI", 12F);
            summarySemesterLabel.ForeColor = darkText;
            summarySemesterLabel.Location = new Point(18, 92);
            summarySemesterLabel.AutoSize = true;

            summaryStatusLabel = new Label();
            summaryStatusLabel.Text = "Status: —";
            summaryStatusLabel.Font = new Font("Segoe UI", 12F);
            summaryStatusLabel.ForeColor = darkText;
            summaryStatusLabel.Location = new Point(18, 124);
            summaryStatusLabel.AutoSize = true;

            summaryYearLevelLabel = new Label();
            summaryYearLevelLabel.Text = "Year Level: —";
            summaryYearLevelLabel.Font = new Font("Segoe UI", 12F);
            summaryYearLevelLabel.ForeColor = darkText;
            summaryYearLevelLabel.Location = new Point(18, 156);
            summaryYearLevelLabel.AutoSize = true;

            summaryPanel.Controls.Add(summaryTitleLabel);
            summaryPanel.Controls.Add(summaryGwaLabel);
            summaryPanel.Controls.Add(summarySemesterLabel);
            summaryPanel.Controls.Add(summaryStatusLabel);
            summaryPanel.Controls.Add(summaryYearLevelLabel);

            summaryTab.Controls.Add(summaryPanel);

            // Wire tabs into TabControl
            gradesTabControl.TabPages.Add(gradesTab);
            gradesTabControl.TabPages.Add(summaryTab);

            Controls.Add(gradesTabControl);
        }

        #endregion

        private TabControl gradesTabControl;
        private TabPage gradesTab;
        private TabPage summaryTab;

        // Grades tab controls
        private Panel gradesHeaderPanel;
        private Label gradesTitle;
        private ComboBox gradesSemesterComboBox;
        private Label gradesSemesterInfoLabel;
        private Panel gradesTableContainer;
        private DataGridView gradesDataGridView;
        private Label gradesEmptyLabel;

        // Summary tab controls
        private Panel summaryPanel;
        private Label summaryTitleLabel;
        private Label summaryGwaLabel;
        private Label summarySemesterLabel;
        private Label summaryStatusLabel;
        private Label summaryYearLevelLabel;
    }
}
