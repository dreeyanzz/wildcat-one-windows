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

            // Add both page panels to content panel
            contentPanel.Controls.Add(dashboardPanel);
            contentPanel.Controls.Add(schedulePagePanel);

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
    }
}
