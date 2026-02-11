namespace wildcat_one_windows
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
            components = new System.ComponentModel.Container();

            var maroon = Color.FromArgb(122, 26, 61);
            var maroonLight = Color.FromArgb(142, 46, 81);
            var darkText = Color.FromArgb(52, 73, 94);
            var contentBg = Color.FromArgb(240, 242, 245);

            // === Form ===
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Wildcat One";
            Font = new Font("Segoe UI", 9F);

            // ===========================================
            // MenuStrip
            // ===========================================
            menuStrip = new MenuStrip();
            menuStrip.BackColor = Color.White;
            menuStrip.Padding = new Padding(6, 2, 0, 2);

            // File menu
            fileMenu = new ToolStripMenuItem("File");
            menuLogout = new ToolStripMenuItem("Logout", null, MenuLogout_Click);
            menuExit = new ToolStripMenuItem("Exit", null, MenuExit_Click);
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { menuLogout, new ToolStripSeparator(), menuExit });

            // View menu
            viewMenu = new ToolStripMenuItem("View");
            menuDashboard = new ToolStripMenuItem("Dashboard", null, MenuDashboard_Click);
            menuSchedule = new ToolStripMenuItem("Schedule", null, MenuSchedule_Click);
            menuGrades = new ToolStripMenuItem("Grades", null, MenuGrades_Click);
            menuProfessors = new ToolStripMenuItem("Professors", null, MenuProfessors_Click);
            menuCourseOfferings = new ToolStripMenuItem("Course Offerings", null, MenuCourseOfferings_Click);
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] { menuDashboard, menuSchedule, menuGrades, menuProfessors, menuCourseOfferings });

            // Account menu
            accountMenu = new ToolStripMenuItem("Account");
            menuChangePassword = new ToolStripMenuItem("Change Password", null, MenuChangePassword_Click);
            accountMenu.DropDownItems.Add(menuChangePassword);

            // Help menu
            helpMenu = new ToolStripMenuItem("Help");
            menuAbout = new ToolStripMenuItem("About", null, MenuAbout_Click);
            helpMenu.DropDownItems.Add(menuAbout);

            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, viewMenu, accountMenu, helpMenu });
            MainMenuStrip = menuStrip;

            // ===========================================
            // ContextMenuStrip (for class cards)
            // ===========================================
            classContextMenu = new ContextMenuStrip(components);
            ctxCopyCourseCode = new ToolStripMenuItem("Copy Course Code", null, CtxCopyCourseCode_Click);
            ctxCopyProfessor = new ToolStripMenuItem("Copy Professor Name", null, CtxCopyProfessor_Click);
            ctxCopyRoom = new ToolStripMenuItem("Copy Room", null, CtxCopyRoom_Click);
            classContextMenu.Items.AddRange(new ToolStripItem[] { ctxCopyCourseCode, ctxCopyProfessor, ctxCopyRoom });

            // ===========================================
            // Sidebar Panel
            // ===========================================
            sidebarPanel = new Panel();
            sidebarPanel.Size = new Size(220, 700 - menuStrip.Height);
            sidebarPanel.Location = new Point(0, menuStrip.Height);
            sidebarPanel.BackColor = maroon;
            sidebarPanel.Dock = DockStyle.None;

            // Sidebar logo container (white bg so maroon logo text is visible)
            sidebarLogoPanel = new Panel();
            sidebarLogoPanel.Size = new Size(196, 46);
            sidebarLogoPanel.Location = new Point(12, 10);
            sidebarLogoPanel.BackColor = Color.White;

            sidebarLogo = new PictureBox();
            sidebarLogo.Size = new Size(190, 40);
            sidebarLogo.SizeMode = PictureBoxSizeMode.Zoom;
            sidebarLogo.BackColor = Color.White;
            sidebarLogo.Location = new Point(3, 3);
            sidebarLogoPanel.Controls.Add(sidebarLogo);

            // Sidebar student name
            sidebarNameLabel = new Label();
            sidebarNameLabel.Text = "Student Name";
            sidebarNameLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            sidebarNameLabel.ForeColor = Color.White;
            sidebarNameLabel.AutoSize = false;
            sidebarNameLabel.Size = new Size(200, 24);
            sidebarNameLabel.Location = new Point(10, 68);
            sidebarNameLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Sidebar student ID
            sidebarIdLabel = new Label();
            sidebarIdLabel.Text = "00-0000-000";
            sidebarIdLabel.Font = new Font("Segoe UI", 9F);
            sidebarIdLabel.ForeColor = Color.FromArgb(200, 255, 255, 255);
            sidebarIdLabel.AutoSize = false;
            sidebarIdLabel.Size = new Size(200, 18);
            sidebarIdLabel.Location = new Point(10, 94);
            sidebarIdLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Sidebar separator
            sidebarSeparator = new Panel();
            sidebarSeparator.Size = new Size(180, 1);
            sidebarSeparator.Location = new Point(20, 122);
            sidebarSeparator.BackColor = Color.FromArgb(80, 255, 255, 255);

            // Navigation buttons
            var navButtonY = 136;
            var navButtonHeight = 42;
            var navButtonWidth = 220;

            btnDashboard = CreateSidebarButton("  Dashboard", navButtonY, maroon, maroonLight);
            btnDashboard.BackColor = maroonLight; // Active by default
            btnDashboard.Click += BtnDashboard_Click;
            navButtonY += navButtonHeight;

            btnSchedule = CreateSidebarButton("  Schedule", navButtonY, maroon, maroonLight);
            btnSchedule.Click += BtnSchedule_Click;
            navButtonY += navButtonHeight;

            btnGrades = CreateSidebarButton("  Grades", navButtonY, maroon, maroonLight);
            btnGrades.Click += BtnGrades_Click;
            navButtonY += navButtonHeight;

            btnProfessors = CreateSidebarButton("  Professors", navButtonY, maroon, maroonLight);
            btnProfessors.Click += BtnProfessors_Click;
            navButtonY += navButtonHeight;

            btnCourseOfferings = CreateSidebarButton("  Course Offerings", navButtonY, maroon, maroonLight);
            btnCourseOfferings.Click += BtnCourseOfferings_Click;
            navButtonY += navButtonHeight;

            btnChangePassword = CreateSidebarButton("  Change Password", navButtonY, maroon, maroonLight);
            btnChangePassword.Click += BtnChangePassword_Click;

            // Logout separator + button at bottom
            sidebarLogoutSeparator = new Panel();
            sidebarLogoutSeparator.Size = new Size(180, 1);
            sidebarLogoutSeparator.BackColor = Color.FromArgb(80, 255, 255, 255);

            btnLogout = new Button();
            btnLogout.Text = "  Logout";
            btnLogout.Font = new Font("Segoe UI", 10F);
            btnLogout.ForeColor = Color.White;
            btnLogout.BackColor = maroon;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = maroonLight;
            btnLogout.Size = new Size(navButtonWidth, navButtonHeight);
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Click += BtnLogout_Click;

            // Add sidebar controls
            sidebarPanel.Controls.Add(sidebarLogoPanel);
            sidebarPanel.Controls.Add(sidebarNameLabel);
            sidebarPanel.Controls.Add(sidebarIdLabel);
            sidebarPanel.Controls.Add(sidebarSeparator);
            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(btnSchedule);
            sidebarPanel.Controls.Add(btnGrades);
            sidebarPanel.Controls.Add(btnProfessors);
            sidebarPanel.Controls.Add(btnCourseOfferings);
            sidebarPanel.Controls.Add(btnChangePassword);
            sidebarPanel.Controls.Add(sidebarLogoutSeparator);
            sidebarPanel.Controls.Add(btnLogout);

            // ===========================================
            // Content Panel
            // ===========================================
            var contentWidth = 1100 - 220;
            var contentHeight = 700 - menuStrip.Height;

            contentPanel = new Panel();
            contentPanel.Location = new Point(220, menuStrip.Height);
            contentPanel.Size = new Size(contentWidth, contentHeight);
            contentPanel.BackColor = contentBg;

            // ===========================================
            // Dashboard Panel (wraps all dashboard controls)
            // ===========================================
            dashboardPanel = new Panel();
            dashboardPanel.Location = new Point(0, 0);
            dashboardPanel.Size = new Size(contentWidth, contentHeight);
            dashboardPanel.BackColor = contentBg;
            dashboardPanel.Padding = new Padding(20);
            dashboardPanel.AutoScroll = true;

            // --- Welcome GroupBox ---
            welcomeGroupBox = new GroupBox();
            welcomeGroupBox.Text = "Welcome";
            welcomeGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            welcomeGroupBox.ForeColor = darkText;
            welcomeGroupBox.Location = new Point(20, 12);
            welcomeGroupBox.Size = new Size(840, 110);
            welcomeGroupBox.BackColor = Color.White;

            welcomeNameLabel = new Label();
            welcomeNameLabel.Text = "Student Name";
            welcomeNameLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            welcomeNameLabel.ForeColor = Color.FromArgb(122, 26, 61);
            welcomeNameLabel.Location = new Point(18, 28);
            welcomeNameLabel.AutoSize = true;

            welcomeDetailsLabel = new Label();
            welcomeDetailsLabel.Text = "Student ID: --- | Academic Year: --- | Term: ---";
            welcomeDetailsLabel.Font = new Font("Segoe UI", 10F);
            welcomeDetailsLabel.ForeColor = Color.FromArgb(100, 100, 100);
            welcomeDetailsLabel.Location = new Point(18, 68);
            welcomeDetailsLabel.AutoSize = true;

            welcomeGroupBox.Controls.Add(welcomeNameLabel);
            welcomeGroupBox.Controls.Add(welcomeDetailsLabel);

            // --- Stats FlowLayoutPanel ---
            statsFlowPanel = new FlowLayoutPanel();
            statsFlowPanel.Location = new Point(20, 132);
            statsFlowPanel.Size = new Size(840, 110);
            statsFlowPanel.BackColor = Color.Transparent;
            statsFlowPanel.WrapContents = false;
            statsFlowPanel.FlowDirection = FlowDirection.LeftToRight;
            statsFlowPanel.Padding = new Padding(0);

            // Stat cards
            statCoursesPanel = CreateStatCard("0", "Current Courses");
            statGwaPanel = CreateStatCard("N/A", "Current GWA");
            statProfessorsPanel = CreateStatCard("0", "Total Professors");
            statSemesterPanel = CreateStatCard("---", "Current Semester");

            statsFlowPanel.Controls.Add(statCoursesPanel);
            statsFlowPanel.Controls.Add(statGwaPanel);
            statsFlowPanel.Controls.Add(statProfessorsPanel);
            statsFlowPanel.Controls.Add(statSemesterPanel);

            // --- Today's Classes GroupBox ---
            todayGroupBox = new GroupBox();
            todayGroupBox.Text = "Today's Classes";
            todayGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            todayGroupBox.ForeColor = darkText;
            todayGroupBox.Location = new Point(20, 252);
            todayGroupBox.Size = new Size(840, 360);
            todayGroupBox.BackColor = Color.White;

            todayClassesPanel = new Panel();
            todayClassesPanel.Location = new Point(10, 26);
            todayClassesPanel.Size = new Size(820, 324);
            todayClassesPanel.AutoScroll = true;
            todayClassesPanel.BackColor = Color.White;

            noClassesLabel = new Label();
            noClassesLabel.Text = "Loading schedule...";
            noClassesLabel.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            noClassesLabel.ForeColor = Color.FromArgb(150, 150, 150);
            noClassesLabel.AutoSize = false;
            noClassesLabel.Size = new Size(800, 40);
            noClassesLabel.Location = new Point(10, 10);
            noClassesLabel.TextAlign = ContentAlignment.MiddleCenter;
            todayClassesPanel.Controls.Add(noClassesLabel);

            todayGroupBox.Controls.Add(todayClassesPanel);

            dashboardPanel.Controls.Add(welcomeGroupBox);
            dashboardPanel.Controls.Add(statsFlowPanel);
            dashboardPanel.Controls.Add(todayGroupBox);

            // ===========================================
            // Schedule Page Panel
            // ===========================================
            schedulePagePanel = new Panel();
            schedulePagePanel.Location = new Point(0, 0);
            schedulePagePanel.Size = new Size(contentWidth, contentHeight);
            schedulePagePanel.BackColor = contentBg;
            schedulePagePanel.Visible = false;

            // --- Schedule Header Panel ---
            scheduleHeaderPanel = new Panel();
            scheduleHeaderPanel.Location = new Point(20, 12);
            scheduleHeaderPanel.Size = new Size(840, 70);
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
            semesterComboBox.Size = new Size(280, 26);

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
            scheduleGridContainer.Size = new Size(840, contentHeight - 112);
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

            schedulePagePanel.Controls.Add(scheduleHeaderPanel);
            schedulePagePanel.Controls.Add(scheduleGridContainer);

            // ===========================================
            // Grades Page Panel
            // ===========================================
            gradesPagePanel = new Panel();
            gradesPagePanel.Location = new Point(0, 0);
            gradesPagePanel.Size = new Size(contentWidth, contentHeight);
            gradesPagePanel.BackColor = contentBg;
            gradesPagePanel.Visible = false;

            // --- Grades Header Panel ---
            gradesHeaderPanel = new Panel();
            gradesHeaderPanel.Location = new Point(20, 12);
            gradesHeaderPanel.Size = new Size(840, 70);
            gradesHeaderPanel.BackColor = Color.White;

            gradesTitle = new Label();
            gradesTitle.Text = "Academic Grades";
            gradesTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            gradesTitle.ForeColor = Color.FromArgb(122, 26, 61);
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
            gradesTableContainer.Size = new Size(840, contentHeight - 112);
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

            // Header styling
            gradesDataGridView.EnableHeadersVisualStyles = false;
            gradesDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = Color.FromArgb(122, 26, 61),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = Color.FromArgb(248, 249, 250),
                SelectionForeColor = Color.FromArgb(122, 26, 61)
            };
            gradesDataGridView.ColumnHeadersHeight = 40;
            gradesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gradesDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Row styling
            gradesDataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = Color.FromArgb(52, 73, 94),
                Padding = new Padding(8, 0, 0, 0),
                WrapMode = DataGridViewTriState.True
            };
            gradesDataGridView.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 250, 252),
                ForeColor = Color.FromArgb(52, 73, 94),
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = Color.FromArgb(52, 73, 94),
                Padding = new Padding(8, 0, 0, 0),
                WrapMode = DataGridViewTriState.True
            };

            // Columns
            var colCourseCode = new DataGridViewTextBoxColumn
            {
                Name = "CourseCode",
                HeaderText = "Course Code",
                Width = 100
            };
            var colCourseTitle = new DataGridViewTextBoxColumn
            {
                Name = "CourseTitle",
                HeaderText = "Course Title",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
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

            // --- Grades empty state label (hidden by default) ---
            gradesEmptyLabel = new Label();
            gradesEmptyLabel.Text = "No grades available";
            gradesEmptyLabel.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            gradesEmptyLabel.ForeColor = Color.FromArgb(150, 150, 150);
            gradesEmptyLabel.AutoSize = false;
            gradesEmptyLabel.Size = new Size(840, 40);
            gradesEmptyLabel.Location = new Point(20, 200);
            gradesEmptyLabel.TextAlign = ContentAlignment.MiddleCenter;
            gradesEmptyLabel.Visible = false;

            gradesPagePanel.Controls.Add(gradesHeaderPanel);
            gradesPagePanel.Controls.Add(gradesTableContainer);
            gradesPagePanel.Controls.Add(gradesEmptyLabel);

            // ===========================================
            // Professors Page Panel
            // ===========================================
            professorsPagePanel = new Panel();
            professorsPagePanel.Location = new Point(0, 0);
            professorsPagePanel.Size = new Size(contentWidth, contentHeight);
            professorsPagePanel.BackColor = contentBg;
            professorsPagePanel.Visible = false;

            // --- Professors Header Panel ---
            profHeaderPanel = new Panel();
            profHeaderPanel.Location = new Point(20, 12);
            profHeaderPanel.Size = new Size(840, 70);
            profHeaderPanel.BackColor = Color.White;

            profTitle = new Label();
            profTitle.Text = "My Professors";
            profTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            profTitle.ForeColor = Color.FromArgb(122, 26, 61);
            profTitle.Location = new Point(18, 8);
            profTitle.AutoSize = true;

            profSemesterComboBox = new ComboBox();
            profSemesterComboBox.Font = new Font("Segoe UI", 9.5F);
            profSemesterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            profSemesterComboBox.Location = new Point(540, 12);
            profSemesterComboBox.Size = new Size(280, 26);

            profSemesterInfoLabel = new Label();
            profSemesterInfoLabel.Text = "";
            profSemesterInfoLabel.Font = new Font("Segoe UI", 9F);
            profSemesterInfoLabel.ForeColor = Color.FromArgb(100, 100, 100);
            profSemesterInfoLabel.Location = new Point(18, 44);
            profSemesterInfoLabel.AutoSize = true;

            profHeaderPanel.Controls.Add(profTitle);
            profHeaderPanel.Controls.Add(profSemesterComboBox);
            profHeaderPanel.Controls.Add(profSemesterInfoLabel);

            // --- Professors Table Container (scrollable) ---
            profTableContainer = new Panel();
            profTableContainer.Location = new Point(20, 92);
            profTableContainer.Size = new Size(840, contentHeight - 112);
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

            // Header styling
            profDataGridView.EnableHeadersVisualStyles = false;
            profDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = Color.FromArgb(122, 26, 61),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = Color.FromArgb(248, 249, 250),
                SelectionForeColor = Color.FromArgb(122, 26, 61)
            };
            profDataGridView.ColumnHeadersHeight = 40;
            profDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            profDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Row styling
            profDataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = Color.FromArgb(52, 73, 94),
                Padding = new Padding(8, 0, 0, 0)
            };
            profDataGridView.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 250, 252),
                ForeColor = Color.FromArgb(52, 73, 94),
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = Color.FromArgb(52, 73, 94),
                Padding = new Padding(8, 0, 0, 0)
            };

            // Columns
            var profColCourseCode = new DataGridViewTextBoxColumn
            {
                Name = "ProfCourseCode",
                HeaderText = "Course Code",
                Width = 120
            };
            var profColCourseTitle = new DataGridViewTextBoxColumn
            {
                Name = "ProfCourseTitle",
                HeaderText = "Course Title",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            var profColProfessor = new DataGridViewTextBoxColumn
            {
                Name = "ProfProfessor",
                HeaderText = "Professor",
                Width = 300
            };

            profDataGridView.Columns.AddRange(profColCourseCode, profColCourseTitle, profColProfessor);

            profTableContainer.Controls.Add(profDataGridView);

            // --- Professors empty state label (hidden by default) ---
            profEmptyLabel = new Label();
            profEmptyLabel.Text = "No professor data available";
            profEmptyLabel.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            profEmptyLabel.ForeColor = Color.FromArgb(150, 150, 150);
            profEmptyLabel.AutoSize = false;
            profEmptyLabel.Size = new Size(840, 40);
            profEmptyLabel.Location = new Point(20, 200);
            profEmptyLabel.TextAlign = ContentAlignment.MiddleCenter;
            profEmptyLabel.Visible = false;

            professorsPagePanel.Controls.Add(profHeaderPanel);
            professorsPagePanel.Controls.Add(profTableContainer);
            professorsPagePanel.Controls.Add(profEmptyLabel);

            // ===========================================
            // Change Password Page Panel
            // ===========================================
            var inputBg = Color.FromArgb(248, 249, 251);
            var labelColor = Color.FromArgb(52, 73, 94);

            changePasswordPagePanel = new Panel();
            changePasswordPagePanel.Location = new Point(0, 0);
            changePasswordPagePanel.Size = new Size(contentWidth, contentHeight);
            changePasswordPagePanel.BackColor = contentBg;
            changePasswordPagePanel.Visible = false;
            changePasswordPagePanel.AutoScroll = true;

            // --- Form Card (centered, white bg, ~420px wide) ---
            cpFormCard = new Panel();
            cpFormCard.Size = new Size(420, 560);
            cpFormCard.Location = new Point((contentWidth - 420) / 2, 30);
            cpFormCard.BackColor = Color.White;
            cpFormCard.Padding = new Padding(30);

            // --- Title ---
            cpTitle = new Label();
            cpTitle.Text = "Change Password";
            cpTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            cpTitle.ForeColor = maroon;
            cpTitle.Location = new Point(30, 20);
            cpTitle.AutoSize = true;

            // --- Error Panel ---
            cpErrorPanel = new Panel();
            cpErrorPanel.Location = new Point(30, 56);
            cpErrorPanel.Size = new Size(360, 0);
            cpErrorPanel.BackColor = Color.FromArgb(255, 238, 238);
            cpErrorPanel.Visible = false;
            cpErrorPanel.Paint += CpErrorPanel_Paint;

            cpErrorLabel = new Label();
            cpErrorLabel.Text = "";
            cpErrorLabel.Font = new Font("Segoe UI", 9F);
            cpErrorLabel.ForeColor = Color.FromArgb(231, 76, 60);
            cpErrorLabel.Location = new Point(14, 8);
            cpErrorLabel.Size = new Size(332, 32);
            cpErrorLabel.AutoSize = false;
            cpErrorPanel.Controls.Add(cpErrorLabel);

            // --- Success Panel ---
            cpSuccessPanel = new Panel();
            cpSuccessPanel.Location = new Point(30, 56);
            cpSuccessPanel.Size = new Size(360, 0);
            cpSuccessPanel.BackColor = Color.FromArgb(239, 255, 239);
            cpSuccessPanel.Visible = false;
            cpSuccessPanel.Paint += CpSuccessPanel_Paint;

            cpSuccessLabel = new Label();
            cpSuccessLabel.Text = "";
            cpSuccessLabel.Font = new Font("Segoe UI", 9F);
            cpSuccessLabel.ForeColor = Color.FromArgb(39, 174, 96);
            cpSuccessLabel.Location = new Point(14, 8);
            cpSuccessLabel.Size = new Size(332, 32);
            cpSuccessLabel.AutoSize = false;
            cpSuccessPanel.Controls.Add(cpSuccessLabel);

            // --- Step 1 Panel ---
            cpStep1Panel = new Panel();
            cpStep1Panel.Location = new Point(30, 66);
            cpStep1Panel.Size = new Size(360, 380);
            cpStep1Panel.BackColor = Color.White;

            // Old Password
            cpOldPasswordLabel = new Label();
            cpOldPasswordLabel.Text = "Current Password";
            cpOldPasswordLabel.Font = new Font("Segoe UI", 10F);
            cpOldPasswordLabel.ForeColor = labelColor;
            cpOldPasswordLabel.Location = new Point(0, 10);
            cpOldPasswordLabel.AutoSize = true;

            cpOldPasswordTextBox = new TextBox();
            cpOldPasswordTextBox.Location = new Point(0, 34);
            cpOldPasswordTextBox.Size = new Size(360, 35);
            cpOldPasswordTextBox.Font = new Font("Segoe UI", 11F);
            cpOldPasswordTextBox.BackColor = inputBg;
            cpOldPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            cpOldPasswordTextBox.UseSystemPasswordChar = true;
            cpOldPasswordTextBox.PlaceholderText = "Enter current password";

            cpToggleOldPassword = new Button();
            cpToggleOldPassword.Text = "SHOW";
            cpToggleOldPassword.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            cpToggleOldPassword.ForeColor = Color.FromArgb(127, 140, 141);
            cpToggleOldPassword.FlatStyle = FlatStyle.Flat;
            cpToggleOldPassword.FlatAppearance.BorderSize = 0;
            cpToggleOldPassword.BackColor = inputBg;
            cpToggleOldPassword.Size = new Size(50, 28);
            cpToggleOldPassword.Location = new Point(306, 38);
            cpToggleOldPassword.Cursor = Cursors.Hand;
            cpToggleOldPassword.Click += CpToggleOldPassword_Click;

            // New Password
            cpNewPasswordLabel = new Label();
            cpNewPasswordLabel.Text = "New Password";
            cpNewPasswordLabel.Font = new Font("Segoe UI", 10F);
            cpNewPasswordLabel.ForeColor = labelColor;
            cpNewPasswordLabel.Location = new Point(0, 80);
            cpNewPasswordLabel.AutoSize = true;

            cpNewPasswordTextBox = new TextBox();
            cpNewPasswordTextBox.Location = new Point(0, 104);
            cpNewPasswordTextBox.Size = new Size(360, 35);
            cpNewPasswordTextBox.Font = new Font("Segoe UI", 11F);
            cpNewPasswordTextBox.BackColor = inputBg;
            cpNewPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            cpNewPasswordTextBox.UseSystemPasswordChar = true;
            cpNewPasswordTextBox.PlaceholderText = "Enter new password";

            cpToggleNewPassword = new Button();
            cpToggleNewPassword.Text = "SHOW";
            cpToggleNewPassword.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            cpToggleNewPassword.ForeColor = Color.FromArgb(127, 140, 141);
            cpToggleNewPassword.FlatStyle = FlatStyle.Flat;
            cpToggleNewPassword.FlatAppearance.BorderSize = 0;
            cpToggleNewPassword.BackColor = inputBg;
            cpToggleNewPassword.Size = new Size(50, 28);
            cpToggleNewPassword.Location = new Point(306, 108);
            cpToggleNewPassword.Cursor = Cursors.Hand;
            cpToggleNewPassword.Click += CpToggleNewPassword_Click;

            // Confirm Password
            cpConfirmPasswordLabel = new Label();
            cpConfirmPasswordLabel.Text = "Confirm New Password";
            cpConfirmPasswordLabel.Font = new Font("Segoe UI", 10F);
            cpConfirmPasswordLabel.ForeColor = labelColor;
            cpConfirmPasswordLabel.Location = new Point(0, 150);
            cpConfirmPasswordLabel.AutoSize = true;

            cpConfirmPasswordTextBox = new TextBox();
            cpConfirmPasswordTextBox.Location = new Point(0, 174);
            cpConfirmPasswordTextBox.Size = new Size(360, 35);
            cpConfirmPasswordTextBox.Font = new Font("Segoe UI", 11F);
            cpConfirmPasswordTextBox.BackColor = inputBg;
            cpConfirmPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            cpConfirmPasswordTextBox.UseSystemPasswordChar = true;
            cpConfirmPasswordTextBox.PlaceholderText = "Confirm new password";

            cpToggleConfirmPassword = new Button();
            cpToggleConfirmPassword.Text = "SHOW";
            cpToggleConfirmPassword.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            cpToggleConfirmPassword.ForeColor = Color.FromArgb(127, 140, 141);
            cpToggleConfirmPassword.FlatStyle = FlatStyle.Flat;
            cpToggleConfirmPassword.FlatAppearance.BorderSize = 0;
            cpToggleConfirmPassword.BackColor = inputBg;
            cpToggleConfirmPassword.Size = new Size(50, 28);
            cpToggleConfirmPassword.Location = new Point(306, 178);
            cpToggleConfirmPassword.Cursor = Cursors.Hand;
            cpToggleConfirmPassword.Click += CpToggleConfirmPassword_Click;

            // Request OTP Button
            cpRequestOtpButton = new Button();
            cpRequestOtpButton.Text = "Request OTP";
            cpRequestOtpButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            cpRequestOtpButton.ForeColor = Color.White;
            cpRequestOtpButton.BackColor = maroon;
            cpRequestOtpButton.FlatStyle = FlatStyle.Flat;
            cpRequestOtpButton.FlatAppearance.BorderSize = 0;
            cpRequestOtpButton.Size = new Size(360, 44);
            cpRequestOtpButton.Location = new Point(0, 230);
            cpRequestOtpButton.Cursor = Cursors.Hand;
            cpRequestOtpButton.Click += CpRequestOtp_Click;

            cpStep1Panel.Controls.Add(cpOldPasswordLabel);
            cpStep1Panel.Controls.Add(cpOldPasswordTextBox);
            cpStep1Panel.Controls.Add(cpToggleOldPassword);
            cpStep1Panel.Controls.Add(cpNewPasswordLabel);
            cpStep1Panel.Controls.Add(cpNewPasswordTextBox);
            cpStep1Panel.Controls.Add(cpToggleNewPassword);
            cpStep1Panel.Controls.Add(cpConfirmPasswordLabel);
            cpStep1Panel.Controls.Add(cpConfirmPasswordTextBox);
            cpStep1Panel.Controls.Add(cpToggleConfirmPassword);
            cpStep1Panel.Controls.Add(cpRequestOtpButton);

            // --- Step 2 Panel ---
            cpStep2Panel = new Panel();
            cpStep2Panel.Location = new Point(30, 66);
            cpStep2Panel.Size = new Size(360, 340);
            cpStep2Panel.BackColor = Color.White;
            cpStep2Panel.Visible = false;

            // OTP Info Label
            cpOtpInfoLabel = new Label();
            cpOtpInfoLabel.Text = "A 6-digit OTP has been sent to your registered email.";
            cpOtpInfoLabel.Font = new Font("Segoe UI", 9.5F);
            cpOtpInfoLabel.ForeColor = Color.FromArgb(100, 100, 100);
            cpOtpInfoLabel.Location = new Point(0, 10);
            cpOtpInfoLabel.Size = new Size(360, 40);
            cpOtpInfoLabel.AutoSize = false;

            // OTP Timer Label
            cpOtpTimerLabel = new Label();
            cpOtpTimerLabel.Text = "OTP expires in 5:00";
            cpOtpTimerLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            cpOtpTimerLabel.ForeColor = maroon;
            cpOtpTimerLabel.Location = new Point(0, 50);
            cpOtpTimerLabel.AutoSize = true;

            // OTP Label
            cpOtpLabel = new Label();
            cpOtpLabel.Text = "Enter OTP";
            cpOtpLabel.Font = new Font("Segoe UI", 10F);
            cpOtpLabel.ForeColor = labelColor;
            cpOtpLabel.Location = new Point(0, 82);
            cpOtpLabel.AutoSize = true;

            // OTP TextBox
            cpOtpTextBox = new TextBox();
            cpOtpTextBox.Location = new Point(0, 106);
            cpOtpTextBox.Size = new Size(360, 35);
            cpOtpTextBox.Font = new Font("Segoe UI", 14F);
            cpOtpTextBox.BackColor = inputBg;
            cpOtpTextBox.BorderStyle = BorderStyle.FixedSingle;
            cpOtpTextBox.MaxLength = 6;
            cpOtpTextBox.PlaceholderText = "000000";
            cpOtpTextBox.TextAlign = HorizontalAlignment.Center;
            cpOtpTextBox.KeyPress += CpOtpTextBox_KeyPress;
            cpOtpTextBox.TextChanged += CpOtpTextBox_TextChanged;

            // Submit Button
            cpSubmitButton = new Button();
            cpSubmitButton.Text = "Change Password";
            cpSubmitButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            cpSubmitButton.ForeColor = Color.White;
            cpSubmitButton.BackColor = maroon;
            cpSubmitButton.FlatStyle = FlatStyle.Flat;
            cpSubmitButton.FlatAppearance.BorderSize = 0;
            cpSubmitButton.Size = new Size(360, 44);
            cpSubmitButton.Location = new Point(0, 162);
            cpSubmitButton.Cursor = Cursors.Hand;
            cpSubmitButton.Click += CpSubmitPasswordChange_Click;

            // Start Over Button
            cpStartOverButton = new Button();
            cpStartOverButton.Text = "Start Over";
            cpStartOverButton.Font = new Font("Segoe UI", 10F);
            cpStartOverButton.ForeColor = Color.FromArgb(127, 140, 141);
            cpStartOverButton.BackColor = Color.White;
            cpStartOverButton.FlatStyle = FlatStyle.Flat;
            cpStartOverButton.FlatAppearance.BorderSize = 1;
            cpStartOverButton.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            cpStartOverButton.Size = new Size(360, 40);
            cpStartOverButton.Location = new Point(0, 216);
            cpStartOverButton.Cursor = Cursors.Hand;
            cpStartOverButton.Click += CpStartOver_Click;

            cpStep2Panel.Controls.Add(cpOtpInfoLabel);
            cpStep2Panel.Controls.Add(cpOtpTimerLabel);
            cpStep2Panel.Controls.Add(cpOtpLabel);
            cpStep2Panel.Controls.Add(cpOtpTextBox);
            cpStep2Panel.Controls.Add(cpSubmitButton);
            cpStep2Panel.Controls.Add(cpStartOverButton);

            // Add controls to form card
            cpFormCard.Controls.Add(cpTitle);
            cpFormCard.Controls.Add(cpErrorPanel);
            cpFormCard.Controls.Add(cpSuccessPanel);
            cpFormCard.Controls.Add(cpStep1Panel);
            cpFormCard.Controls.Add(cpStep2Panel);

            changePasswordPagePanel.Controls.Add(cpFormCard);

            // ===========================================
            // Course Offerings Page Panel
            // ===========================================
            courseOfferingsPagePanel = new Panel();
            courseOfferingsPagePanel.Location = new Point(0, 0);
            courseOfferingsPagePanel.Size = new Size(contentWidth, contentHeight);
            courseOfferingsPagePanel.BackColor = contentBg;
            courseOfferingsPagePanel.Visible = false;
            courseOfferingsPagePanel.AutoScroll = true;

            // --- Course Offerings Header Panel ---
            coHeaderPanel = new Panel();
            coHeaderPanel.Location = new Point(20, 12);
            coHeaderPanel.Size = new Size(840, 70);
            coHeaderPanel.BackColor = Color.White;

            coTitle = new Label();
            coTitle.Text = "Course Offerings";
            coTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            coTitle.ForeColor = Color.FromArgb(122, 26, 61);
            coTitle.Location = new Point(18, 18);
            coTitle.AutoSize = true;

            coHeaderPanel.Controls.Add(coTitle);

            // --- Course Offerings Search Panel ---
            coSearchPanel = new Panel();
            coSearchPanel.Location = new Point(20, 92);
            coSearchPanel.Size = new Size(840, 70);
            coSearchPanel.BackColor = Color.White;

            coSearchLabel = new Label();
            coSearchLabel.Text = "Search Course:";
            coSearchLabel.Font = new Font("Segoe UI", 10F);
            coSearchLabel.ForeColor = Color.FromArgb(52, 73, 94);
            coSearchLabel.Location = new Point(18, 8);
            coSearchLabel.AutoSize = true;

            coSearchTextBox = new TextBox();
            coSearchTextBox.Location = new Point(18, 32);
            coSearchTextBox.Size = new Size(500, 28);
            coSearchTextBox.Font = new Font("Segoe UI", 10F);
            coSearchTextBox.BackColor = inputBg;
            coSearchTextBox.BorderStyle = BorderStyle.FixedSingle;
            coSearchTextBox.PlaceholderText = "Type course code or name...";

            coSearchButton = new Button();
            coSearchButton.Text = "Search";
            coSearchButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            coSearchButton.ForeColor = Color.White;
            coSearchButton.BackColor = maroon;
            coSearchButton.FlatStyle = FlatStyle.Flat;
            coSearchButton.FlatAppearance.BorderSize = 0;
            coSearchButton.Size = new Size(90, 30);
            coSearchButton.Location = new Point(528, 31);
            coSearchButton.Cursor = Cursors.Hand;

            coSearchPanel.Controls.Add(coSearchLabel);
            coSearchPanel.Controls.Add(coSearchTextBox);
            coSearchPanel.Controls.Add(coSearchButton);

            // --- Course Offerings Dropdown ListBox (autocomplete) ---
            coDropdownListBox = new ListBox();
            coDropdownListBox.Location = new Point(38, 162);
            coDropdownListBox.Size = new Size(500, 0);
            coDropdownListBox.Font = new Font("Segoe UI", 9.5F);
            coDropdownListBox.BorderStyle = BorderStyle.FixedSingle;
            coDropdownListBox.Visible = false;

            // --- Loading Label ---
            coLoadingLabel = new Label();
            coLoadingLabel.Text = "Loading courses...";
            coLoadingLabel.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            coLoadingLabel.ForeColor = Color.FromArgb(150, 150, 150);
            coLoadingLabel.AutoSize = false;
            coLoadingLabel.Size = new Size(840, 40);
            coLoadingLabel.Location = new Point(20, 200);
            coLoadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            coLoadingLabel.Visible = false;

            // --- Error Label ---
            coErrorLabel = new Label();
            coErrorLabel.Text = "";
            coErrorLabel.Font = new Font("Segoe UI", 10F);
            coErrorLabel.ForeColor = Color.FromArgb(231, 76, 60);
            coErrorLabel.AutoSize = false;
            coErrorLabel.Size = new Size(840, 40);
            coErrorLabel.Location = new Point(20, 200);
            coErrorLabel.TextAlign = ContentAlignment.MiddleCenter;
            coErrorLabel.Visible = false;

            // --- Info Panel (shows selected course info) ---
            coInfoPanel = new Panel();
            coInfoPanel.Location = new Point(20, 172);
            coInfoPanel.Size = new Size(840, 40);
            coInfoPanel.BackColor = Color.White;
            coInfoPanel.Visible = false;

            coInfoLabel = new Label();
            coInfoLabel.Text = "";
            coInfoLabel.Font = new Font("Segoe UI", 10F);
            coInfoLabel.ForeColor = Color.FromArgb(52, 73, 94);
            coInfoLabel.Location = new Point(18, 10);
            coInfoLabel.AutoSize = true;

            coInfoPanel.Controls.Add(coInfoLabel);

            // --- Table Container ---
            coTableContainer = new Panel();
            coTableContainer.Location = new Point(20, 222);
            coTableContainer.Size = new Size(840, contentHeight - 242);
            coTableContainer.AutoScroll = true;
            coTableContainer.BackColor = Color.White;
            coTableContainer.Visible = false;

            // --- DataGridView ---
            coDataGridView = new DataGridView();
            coDataGridView.Dock = DockStyle.Fill;
            coDataGridView.ReadOnly = true;
            coDataGridView.AllowUserToAddRows = false;
            coDataGridView.AllowUserToDeleteRows = false;
            coDataGridView.AllowUserToResizeRows = false;
            coDataGridView.RowHeadersVisible = false;
            coDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            coDataGridView.MultiSelect = false;
            coDataGridView.BorderStyle = BorderStyle.None;
            coDataGridView.BackgroundColor = Color.White;
            coDataGridView.GridColor = Color.FromArgb(230, 230, 230);
            coDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            coDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            coDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            coDataGridView.RowTemplate.Height = 70;
            coDataGridView.Font = new Font("Segoe UI", 9F);

            // Header styling
            coDataGridView.EnableHeadersVisualStyles = false;
            coDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = Color.FromArgb(122, 26, 61),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = Color.FromArgb(248, 249, 250),
                SelectionForeColor = Color.FromArgb(122, 26, 61)
            };
            coDataGridView.ColumnHeadersHeight = 40;
            coDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            coDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Row styling
            coDataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = Color.FromArgb(52, 73, 94),
                Padding = new Padding(8, 4, 8, 4),
                WrapMode = DataGridViewTriState.True
            };
            coDataGridView.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 250, 252),
                ForeColor = Color.FromArgb(52, 73, 94),
                SelectionBackColor = Color.FromArgb(240, 242, 245),
                SelectionForeColor = Color.FromArgb(52, 73, 94),
                Padding = new Padding(8, 4, 8, 4),
                WrapMode = DataGridViewTriState.True
            };

            // Columns — wider widths so the grid scrolls horizontally
            var coColSection = new DataGridViewTextBoxColumn
            {
                Name = "Section",
                HeaderText = "Section",
                Width = 90,
                MinimumWidth = 70
            };
            var coColSchedule = new DataGridViewTextBoxColumn
            {
                Name = "Schedule",
                HeaderText = "Schedule",
                Width = 220,
                MinimumWidth = 180
            };
            var coColSlots = new DataGridViewTextBoxColumn
            {
                Name = "Slots",
                HeaderText = "Slots",
                Width = 100,
                MinimumWidth = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            var coColFaculty = new DataGridViewTextBoxColumn
            {
                Name = "Faculty",
                HeaderText = "Faculty",
                Width = 220,
                MinimumWidth = 150
            };
            var coColMode = new DataGridViewTextBoxColumn
            {
                Name = "Mode",
                HeaderText = "Mode",
                Width = 100,
                MinimumWidth = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            var coColStatus = new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                Width = 90,
                MinimumWidth = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            var coColRemarks = new DataGridViewTextBoxColumn
            {
                Name = "Remarks",
                HeaderText = "Remarks",
                Width = 160,
                MinimumWidth = 100
            };

            coDataGridView.Columns.AddRange(coColSection, coColSchedule, coColSlots, coColFaculty, coColMode, coColStatus, coColRemarks);

            coTableContainer.Controls.Add(coDataGridView);

            // --- Empty Label ---
            coEmptyLabel = new Label();
            coEmptyLabel.Text = "No offerings available for this course";
            coEmptyLabel.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            coEmptyLabel.ForeColor = Color.FromArgb(150, 150, 150);
            coEmptyLabel.AutoSize = false;
            coEmptyLabel.Size = new Size(840, 40);
            coEmptyLabel.Location = new Point(20, 280);
            coEmptyLabel.TextAlign = ContentAlignment.MiddleCenter;
            coEmptyLabel.Visible = false;

            courseOfferingsPagePanel.Controls.Add(coHeaderPanel);
            courseOfferingsPagePanel.Controls.Add(coSearchPanel);
            courseOfferingsPagePanel.Controls.Add(coDropdownListBox);
            courseOfferingsPagePanel.Controls.Add(coLoadingLabel);
            courseOfferingsPagePanel.Controls.Add(coErrorLabel);
            courseOfferingsPagePanel.Controls.Add(coInfoPanel);
            courseOfferingsPagePanel.Controls.Add(coTableContainer);
            courseOfferingsPagePanel.Controls.Add(coEmptyLabel);

            // Bring dropdown to front so it overlays other controls
            coDropdownListBox.BringToFront();

            // Add all page panels to content panel
            contentPanel.Controls.Add(dashboardPanel);
            contentPanel.Controls.Add(schedulePagePanel);
            contentPanel.Controls.Add(gradesPagePanel);
            contentPanel.Controls.Add(professorsPagePanel);
            contentPanel.Controls.Add(changePasswordPagePanel);
            contentPanel.Controls.Add(courseOfferingsPagePanel);

            // ===========================================
            // Add all to form
            // ===========================================
            Controls.Add(menuStrip);
            Controls.Add(sidebarPanel);
            Controls.Add(contentPanel);
        }

        private Button CreateSidebarButton(string text, int y, Color bgColor, Color hoverColor)
        {
            var btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 10F);
            btn.ForeColor = Color.White;
            btn.BackColor = bgColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
            btn.Size = new Size(220, 42);
            btn.Location = new Point(0, y);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private Panel CreateStatCard(string value, string label)
        {
            var card = new Panel();
            card.Size = new Size(198, 90);
            card.BackColor = Color.White;
            card.Margin = new Padding(0, 0, 12, 0);
            card.Paint += StatCard_Paint;

            var valLabel = new Label();
            valLabel.Text = value;
            valLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            valLabel.ForeColor = Color.FromArgb(122, 26, 61);
            valLabel.Location = new Point(14, 14);
            valLabel.AutoSize = false;
            valLabel.Size = new Size(170, 42);
            valLabel.AutoEllipsis = true;
            valLabel.Tag = "statValue";

            var descLabel = new Label();
            descLabel.Text = label;
            descLabel.Font = new Font("Segoe UI", 9F);
            descLabel.ForeColor = Color.FromArgb(120, 120, 120);
            descLabel.Location = new Point(14, 62);
            descLabel.AutoSize = true;

            card.Controls.Add(valLabel);
            card.Controls.Add(descLabel);

            return card;
        }

        #endregion

        // MenuStrip
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem menuLogout;
        private ToolStripMenuItem menuExit;
        private ToolStripMenuItem viewMenu;
        private ToolStripMenuItem menuDashboard;
        private ToolStripMenuItem menuSchedule;
        private ToolStripMenuItem menuGrades;
        private ToolStripMenuItem menuProfessors;
        private ToolStripMenuItem menuCourseOfferings;
        private ToolStripMenuItem accountMenu;
        private ToolStripMenuItem menuChangePassword;
        private ToolStripMenuItem helpMenu;
        private ToolStripMenuItem menuAbout;

        // ContextMenuStrip
        private ContextMenuStrip classContextMenu;
        private ToolStripMenuItem ctxCopyCourseCode;
        private ToolStripMenuItem ctxCopyProfessor;
        private ToolStripMenuItem ctxCopyRoom;

        // Sidebar
        private Panel sidebarPanel;
        private Panel sidebarLogoPanel;
        private PictureBox sidebarLogo;
        private Label sidebarNameLabel;
        private Label sidebarIdLabel;
        private Panel sidebarSeparator;
        private Button btnDashboard;
        private Button btnSchedule;
        private Button btnGrades;
        private Button btnProfessors;
        private Button btnCourseOfferings;
        private Button btnChangePassword;
        private Panel sidebarLogoutSeparator;
        private Button btnLogout;

        // Content
        private Panel contentPanel;

        // Dashboard page
        private Panel dashboardPanel;
        private GroupBox welcomeGroupBox;
        private Label welcomeNameLabel;
        private Label welcomeDetailsLabel;
        private FlowLayoutPanel statsFlowPanel;
        private Panel statCoursesPanel;
        private Panel statGwaPanel;
        private Panel statProfessorsPanel;
        private Panel statSemesterPanel;
        private GroupBox todayGroupBox;
        private Panel todayClassesPanel;
        private Label noClassesLabel;

        // Schedule page
        private Panel schedulePagePanel;
        private Panel scheduleHeaderPanel;
        private Label scheduleTitle;
        private ComboBox semesterComboBox;
        private Label semesterInfoLabel;
        private Panel scheduleGridContainer;
        private DoubleBufferedPanel scheduleGridPanel;

        // Grades page
        private Panel gradesPagePanel;
        private Panel gradesHeaderPanel;
        private Label gradesTitle;
        private ComboBox gradesSemesterComboBox;
        private Label gradesSemesterInfoLabel;
        private Panel gradesTableContainer;
        private DataGridView gradesDataGridView;
        private Label gradesEmptyLabel;

        // Professors page
        private Panel professorsPagePanel;
        private Panel profHeaderPanel;
        private Label profTitle;
        private ComboBox profSemesterComboBox;
        private Label profSemesterInfoLabel;
        private Panel profTableContainer;
        private DataGridView profDataGridView;
        private Label profEmptyLabel;

        // Course Offerings page
        private Panel courseOfferingsPagePanel;
        private Panel coHeaderPanel;
        private Label coTitle;
        private Panel coSearchPanel;
        private Label coSearchLabel;
        private TextBox coSearchTextBox;
        private Button coSearchButton;
        private ListBox coDropdownListBox;
        private Label coLoadingLabel;
        private Label coErrorLabel;
        private Panel coInfoPanel;
        private Label coInfoLabel;
        private Panel coTableContainer;
        private DataGridView coDataGridView;
        private Label coEmptyLabel;

        // Change Password page
        private Panel changePasswordPagePanel;
        private Panel cpFormCard;
        private Label cpTitle;
        private Panel cpErrorPanel;
        private Label cpErrorLabel;
        private Panel cpSuccessPanel;
        private Label cpSuccessLabel;
        private Panel cpStep1Panel;
        private Label cpOldPasswordLabel;
        private TextBox cpOldPasswordTextBox;
        private Button cpToggleOldPassword;
        private Label cpNewPasswordLabel;
        private TextBox cpNewPasswordTextBox;
        private Button cpToggleNewPassword;
        private Label cpConfirmPasswordLabel;
        private TextBox cpConfirmPasswordTextBox;
        private Button cpToggleConfirmPassword;
        private Button cpRequestOtpButton;
        private Panel cpStep2Panel;
        private Label cpOtpInfoLabel;
        private Label cpOtpTimerLabel;
        private Label cpOtpLabel;
        private TextBox cpOtpTextBox;
        private Button cpSubmitButton;
        private Button cpStartOverButton;
    }
}
