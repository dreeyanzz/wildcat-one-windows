namespace wildcat_one_windows
{
    partial class ProfessorsForm
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
            components = new System.ComponentModel.Container();

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
            Text = "My Professors";
            Font = new Font("Segoe UI", 9F);
            BackColor = contentBg;

            // ===========================================
            // ContextMenuStrip (copy professor name)
            // ===========================================
            profContextMenu = new ContextMenuStrip(components);
            ctxCopyProfName = new ToolStripMenuItem("Copy Professor Name", null, CtxCopyProfName_Click);
            ctxCopyCourseCode = new ToolStripMenuItem("Copy Course Code", null, CtxCopyCourseCode_Click);
            profContextMenu.Items.AddRange(new ToolStripItem[] { ctxCopyProfName, ctxCopyCourseCode });

            // ===========================================
            // Professors Header Panel
            // ===========================================
            profHeaderPanel = new Panel();
            profHeaderPanel.Location = new Point(20, 12);
            profHeaderPanel.Size = new Size(860, 70);
            profHeaderPanel.BackColor = Color.White;

            profTitle = new Label();
            profTitle.Text = "My Professors";
            profTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            profTitle.ForeColor = maroon;
            profTitle.Location = new Point(18, 8);
            profTitle.AutoSize = true;

            profSemesterComboBox = new ComboBox();
            profSemesterComboBox.Font = new Font("Segoe UI", 9.5F);
            profSemesterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            profSemesterComboBox.Location = new Point(540, 12);
            profSemesterComboBox.Size = new Size(300, 26);

            profSemesterInfoLabel = new Label();
            profSemesterInfoLabel.Text = "";
            profSemesterInfoLabel.Font = new Font("Segoe UI", 9F);
            profSemesterInfoLabel.ForeColor = Color.FromArgb(100, 100, 100);
            profSemesterInfoLabel.Location = new Point(18, 44);
            profSemesterInfoLabel.AutoSize = true;

            profHeaderPanel.Controls.Add(profTitle);
            profHeaderPanel.Controls.Add(profSemesterComboBox);
            profHeaderPanel.Controls.Add(profSemesterInfoLabel);

            // ===========================================
            // Professors Table Container (scrollable)
            // ===========================================
            profTableContainer = new Panel();
            profTableContainer.Location = new Point(20, 92);
            profTableContainer.Size = new Size(860, formHeight - 112);
            profTableContainer.AutoScroll = true;
            profTableContainer.BackColor = Color.White;

            // --- Professors DataGridView ---
            profDataGridView = new DataGridView();
            profDataGridView.Dock = DockStyle.Fill;
            profDataGridView.ReadOnly = true;
            profDataGridView.AllowUserToAddRows = false;
            profDataGridView.AllowUserToDeleteRows = false;
            profDataGridView.AllowUserToResizeRows = false;
            profDataGridView.RowHeadersVisible = false;
            profDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            profDataGridView.MultiSelect = false;
            profDataGridView.BorderStyle = BorderStyle.None;
            profDataGridView.BackgroundColor = Color.White;
            profDataGridView.GridColor = Color.FromArgb(230, 230, 230);
            profDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            profDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            profDataGridView.RowTemplate.Height = 44;
            profDataGridView.Font = new Font("Segoe UI", 9F);
            profDataGridView.ContextMenuStrip = profContextMenu;

            profDataGridView.EnableHeadersVisualStyles = false;
            profDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = maroon,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = Color.FromArgb(248, 249, 250),
                SelectionForeColor = maroon
            };
            profDataGridView.ColumnHeadersHeight = 40;
            profDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            profDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            profDataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = darkText,
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = darkText,
                Padding = new Padding(8, 0, 0, 0)
            };
            profDataGridView.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 250, 252),
                ForeColor = darkText,
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = darkText,
                Padding = new Padding(8, 0, 0, 0)
            };

            var profColCourseCode = new DataGridViewTextBoxColumn { Name = "ProfCourseCode", HeaderText = "Course Code", Width = 120 };
            var profColCourseTitle = new DataGridViewTextBoxColumn { Name = "ProfCourseTitle", HeaderText = "Course Title", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill };
            var profColProfessor = new DataGridViewTextBoxColumn { Name = "ProfProfessor", HeaderText = "Professor", Width = 300 };

            profDataGridView.Columns.AddRange(profColCourseCode, profColCourseTitle, profColProfessor);
            profTableContainer.Controls.Add(profDataGridView);

            // --- Professors empty state label ---
            profEmptyLabel = new Label();
            profEmptyLabel.Text = "No professor data available";
            profEmptyLabel.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            profEmptyLabel.ForeColor = Color.FromArgb(150, 150, 150);
            profEmptyLabel.AutoSize = false;
            profEmptyLabel.Size = new Size(860, 40);
            profEmptyLabel.Location = new Point(20, 200);
            profEmptyLabel.TextAlign = ContentAlignment.MiddleCenter;
            profEmptyLabel.Visible = false;

            Controls.Add(profHeaderPanel);
            Controls.Add(profTableContainer);
            Controls.Add(profEmptyLabel);
        }

        #endregion

        private ContextMenuStrip profContextMenu;
        private ToolStripMenuItem ctxCopyProfName;
        private ToolStripMenuItem ctxCopyCourseCode;

        private Panel profHeaderPanel;
        private Label profTitle;
        private ComboBox profSemesterComboBox;
        private Label profSemesterInfoLabel;
        private Panel profTableContainer;
        private DataGridView profDataGridView;
        private Label profEmptyLabel;
    }
}
