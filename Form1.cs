using System.Text.Json;
using wildcat_one_windows.Services;

namespace wildcat_one_windows
{
    public partial class Form1 : Form
    {
        private readonly Color _maroon = Color.FromArgb(122, 26, 61);
        private readonly Color _maroonLight = Color.FromArgb(142, 46, 81);
        private Button _activeSidebarButton = null!;
        private JsonElement? _scheduleData;

        public Form1()
        {
            InitializeComponent();
            LoadSidebarLogo();
            PopulateStudentInfo();
            PositionLogoutButton();
            _activeSidebarButton = btnDashboard;
            Load += async (_, _) => await LoadDashboardDataAsync();
        }

        // ===========================================
        // Initialization
        // ===========================================

        private void LoadSidebarLogo()
        {
            var logoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "cit-logo.png");
            if (File.Exists(logoPath))
                sidebarLogo.Image = Image.FromFile(logoPath);
        }

        private void PopulateStudentInfo()
        {
            var session = SessionManager.Instance;
            var userData = session.UserData;

            var firstName = "Student";
            var lastName = "";
            var visibleId = "---";

            if (userData is JsonElement ud)
            {
                if (ud.TryGetProperty("firstName", out var fn)) firstName = fn.ToString();
                if (ud.TryGetProperty("lastName", out var ln)) lastName = ln.ToString();
                if (ud.TryGetProperty("studentIdNumber", out var sid)) visibleId = sid.ToString();
                else if (ud.TryGetProperty("userId", out var uid)) visibleId = uid.ToString();
            }

            var fullName = $"{firstName} {lastName}".Trim();
            var yearName = session.Get<string>("currentAcademicYearName") ?? "---";
            var termName = session.Get<string>("currentTermName") ?? "---";

            sidebarNameLabel.Text = fullName;
            sidebarIdLabel.Text = visibleId;
            welcomeNameLabel.Text = fullName;
            welcomeDetailsLabel.Text = $"Student ID: {visibleId}  |  Academic Year: {yearName}  |  Term: {termName}";
        }

        private void PositionLogoutButton()
        {
            var sidebarHeight = sidebarPanel.Height;
            btnLogout.Location = new Point(0, sidebarHeight - btnLogout.Height - 10);
            sidebarLogoutSeparator.Location = new Point(20, btnLogout.Top - 8);
        }

        // ===========================================
        // Dashboard Data Loading
        // ===========================================

        private async Task LoadDashboardDataAsync()
        {
            try
            {
                _scheduleData = await DashboardService.FetchScheduleAsync();
                PopulateScheduleStats();
                PopulateTodayClasses();
            }
            catch
            {
                noClassesLabel.Text = "Failed to load schedule.";
            }

            try
            {
                var grades = await DashboardService.FetchGradesAsync();
                PopulateGwaAndSemester(grades);
            }
            catch
            {
                UpdateStatValue(statGwaPanel, "N/A");
                UpdateStatValue(statSemesterPanel, "N/A");
            }
        }

        private void PopulateScheduleStats()
        {
            if (_scheduleData is not JsonElement schedule)
            {
                UpdateStatValue(statCoursesPanel, "0");
                UpdateStatValue(statProfessorsPanel, "0");
                return;
            }

            var courses = new HashSet<string>();
            var professors = new HashSet<string>();

            foreach (var item in schedule.EnumerateArray())
            {
                if (item.TryGetProperty("courseCode", out var cc))
                    courses.Add(cc.GetString() ?? "");
                if (item.TryGetProperty("instructor", out var inst))
                    professors.Add(inst.GetString() ?? "");
            }

            UpdateStatValue(statCoursesPanel, courses.Count.ToString());
            UpdateStatValue(statProfessorsPanel, professors.Count.ToString());
        }

        private void PopulateTodayClasses()
        {
            todayClassesPanel.Controls.Clear();

            if (_scheduleData is not JsonElement schedule)
            {
                ShowNoClassesMessage("No classes scheduled for today.");
                return;
            }

            // Day codes: SU=0, M=1, T=2, W=3, TH=4, F=5, S=6
            var todayCode = DateTime.Now.DayOfWeek switch
            {
                DayOfWeek.Sunday    => "SU",
                DayOfWeek.Monday    => "M",
                DayOfWeek.Tuesday   => "T",
                DayOfWeek.Wednesday => "W",
                DayOfWeek.Thursday  => "TH",
                DayOfWeek.Friday    => "F",
                DayOfWeek.Saturday  => "S",
                _ => ""
            };

            // Each item has a "schedule" array, each entry has a "day" array of {code: "M"} objects
            var todayClasses = new List<(JsonElement course, JsonElement sched)>();

            foreach (var course in schedule.EnumerateArray())
            {
                if (!course.TryGetProperty("schedule", out var schedArr) || schedArr.ValueKind != JsonValueKind.Array)
                    continue;

                foreach (var sched in schedArr.EnumerateArray())
                {
                    if (!sched.TryGetProperty("day", out var dayArr) || dayArr.ValueKind != JsonValueKind.Array)
                        continue;

                    foreach (var d in dayArr.EnumerateArray())
                    {
                        if (d.TryGetProperty("code", out var codeProp) &&
                            codeProp.GetString()?.Equals(todayCode, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            todayClasses.Add((course, sched));
                            break;
                        }
                    }
                }
            }

            if (todayClasses.Count == 0)
            {
                ShowNoClassesMessage("No classes scheduled for today.");
                return;
            }

            // Sort by timeFrom
            todayClasses.Sort((a, b) =>
            {
                var ta = TryGetString(a.sched, "timeFrom") ?? "";
                var tb = TryGetString(b.sched, "timeFrom") ?? "";
                return ParseTimeMinutes(ta).CompareTo(ParseTimeMinutes(tb));
            });

            var yOffset = 5;
            foreach (var (course, sched) in todayClasses)
            {
                var card = CreateClassCard(course, sched, yOffset);
                todayClassesPanel.Controls.Add(card);
                yOffset += card.Height + 8;
            }
        }

        private static int ParseTimeMinutes(string timeStr)
        {
            if (string.IsNullOrEmpty(timeStr)) return 0;
            var match = System.Text.RegularExpressions.Regex.Match(timeStr, @"(\d{1,2}):(\d{2})\s*(AM|PM)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (!match.Success) return 0;
            var hours = int.Parse(match.Groups[1].Value);
            var minutes = int.Parse(match.Groups[2].Value);
            var period = match.Groups[3].Value.ToUpper();
            if (period == "PM" && hours != 12) hours += 12;
            else if (period == "AM" && hours == 12) hours = 0;
            return hours * 60 + minutes;
        }

        private void ShowNoClassesMessage(string message)
        {
            noClassesLabel.Text = message;
            todayClassesPanel.Controls.Add(noClassesLabel);
        }

        private Panel CreateClassCard(JsonElement course, JsonElement sched, int yOffset)
        {
            var courseCode = TryGetString(course, "courseCode") ?? "---";
            var courseName = TryGetString(course, "description") ?? "---";
            var section = TryGetString(course, "section") ?? "";
            var professor = TryGetString(course, "instructor") ?? "---";
            var timeFrom = TryGetString(sched, "timeFrom") ?? "";
            var timeTo = TryGetString(sched, "timeTo") ?? "";
            var room = TryGetString(sched, "roomName") ?? "---";
            var time = (timeFrom != "" && timeTo != "") ? $"{timeFrom} - {timeTo}" : "";

            var card = new Panel();
            card.Size = new Size(790, 70);
            card.Location = new Point(5, yOffset);
            card.BackColor = Color.FromArgb(250, 250, 252);
            card.Cursor = Cursors.Default;
            card.Tag = new ClassCardData(courseCode, professor, room);
            card.ContextMenuStrip = classContextMenu;
            card.Paint += ClassCard_Paint;

            var codeLabel = new Label
            {
                Text = section != "" ? $"{courseCode} - {section}" : courseCode,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(122, 26, 61),
                Location = new Point(14, 8),
                AutoSize = true
            };
            codeLabel.ContextMenuStrip = classContextMenu;

            var nameLabel = new Label
            {
                Text = courseName,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(14, 34),
                AutoSize = true
            };
            nameLabel.ContextMenuStrip = classContextMenu;

            var timeLabel = new Label
            {
                Text = time,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(400, 10),
                AutoSize = true
            };
            timeLabel.ContextMenuStrip = classContextMenu;

            var roomLabel = new Label
            {
                Text = $"Room: {room}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(400, 34),
                AutoSize = true
            };
            roomLabel.ContextMenuStrip = classContextMenu;

            var profLabel = new Label
            {
                Text = professor,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(120, 120, 120),
                Location = new Point(600, 10),
                AutoSize = true
            };
            profLabel.ContextMenuStrip = classContextMenu;

            card.Controls.Add(codeLabel);
            card.Controls.Add(nameLabel);
            card.Controls.Add(timeLabel);
            card.Controls.Add(roomLabel);
            card.Controls.Add(profLabel);

            return card;
        }

        private static string? TryGetString(JsonElement el, string prop)
        {
            return el.TryGetProperty(prop, out var val) && val.ValueKind == JsonValueKind.String
                ? val.GetString()
                : null;
        }

        // ===========================================
        // GWA & Semester (from grades API)
        // ===========================================

        private void PopulateGwaAndSemester(JsonElement? grades)
        {
            if (grades is not JsonElement data)
            {
                UpdateStatValue(statGwaPanel, "N/A");
                UpdateStatValue(statSemesterPanel, "N/A");
                return;
            }

            // grades API returns { items: { studentEnrollments: [...] } }
            // data is already "items" from DashboardService
            if (data.TryGetProperty("studentEnrollments", out var enrollments) &&
                enrollments.ValueKind == JsonValueKind.Array)
            {
                var list = enrollments.EnumerateArray().ToList();
                if (list.Count > 0)
                {
                    var current = list[0];

                    // GWA
                    if (current.TryGetProperty("gwa", out var gwaProp))
                    {
                        if (double.TryParse(gwaProp.ToString(), out var gwa) && gwa > 0)
                            UpdateStatValue(statGwaPanel, gwa.ToString("F2"));
                        else
                            UpdateStatValue(statGwaPanel, "N/A");
                    }
                    else
                    {
                        UpdateStatValue(statGwaPanel, "N/A");
                    }

                    // Semester
                    if (current.TryGetProperty("term", out var termProp) && termProp.ValueKind == JsonValueKind.String)
                        UpdateStatValue(statSemesterPanel, termProp.GetString() ?? "N/A");
                    else
                        UpdateStatValue(statSemesterPanel, "N/A");

                    return;
                }
            }

            UpdateStatValue(statGwaPanel, "N/A");
            UpdateStatValue(statSemesterPanel, "N/A");
        }

        private static void UpdateStatValue(Panel statPanel, string value)
        {
            foreach (Control ctrl in statPanel.Controls)
            {
                if (ctrl is Label lbl && lbl.Tag is string tag && tag == "statValue")
                {
                    lbl.Text = value;
                    // Shrink font for long text values (e.g. semester names)
                    if (value.Length > 8)
                        lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    else if (value.Length > 4)
                        lbl.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                    else
                        lbl.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
                    break;
                }
            }
        }

        // ===========================================
        // Sidebar Button Handlers
        // ===========================================

        private void SetActiveSidebarButton(Button btn)
        {
            _activeSidebarButton.BackColor = _maroon;
            btn.BackColor = _maroonLight;
            _activeSidebarButton = btn;
        }

        private void BtnDashboard_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnDashboard);
        }

        private void BtnSchedule_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnSchedule);
            MessageBox.Show("Coming soon!", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnGrades_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnGrades);
            MessageBox.Show("Coming soon!", "Grades", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnProfessors_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnProfessors);
            MessageBox.Show("Coming soon!", "Professors", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCourseOfferings_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnCourseOfferings);
            MessageBox.Show("Coming soon!", "Course Offerings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnChangePassword_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnChangePassword);
            MessageBox.Show("Coming soon!", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            PerformLogout();
        }

        // ===========================================
        // MenuStrip Handlers
        // ===========================================

        private void MenuLogout_Click(object? sender, EventArgs e) => PerformLogout();
        private void MenuExit_Click(object? sender, EventArgs e) => Application.Exit();

        private void MenuDashboard_Click(object? sender, EventArgs e) => BtnDashboard_Click(sender, e);
        private void MenuSchedule_Click(object? sender, EventArgs e) => BtnSchedule_Click(sender, e);
        private void MenuGrades_Click(object? sender, EventArgs e) => BtnGrades_Click(sender, e);
        private void MenuProfessors_Click(object? sender, EventArgs e) => BtnProfessors_Click(sender, e);
        private void MenuCourseOfferings_Click(object? sender, EventArgs e) => BtnCourseOfferings_Click(sender, e);
        private void MenuChangePassword_Click(object? sender, EventArgs e) => BtnChangePassword_Click(sender, e);

        private void MenuAbout_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Wildcat One v1.0\nCIT-U Student Portal", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===========================================
        // ContextMenuStrip Handlers
        // ===========================================

        private ClassCardData? GetClassCardDataFromContext(object? sender)
        {
            if (sender is not ToolStripMenuItem menuItem) return null;
            if (menuItem.Owner is not ContextMenuStrip ctx) return null;
            var source = ctx.SourceControl;
            while (source is not null)
            {
                if (source.Tag is ClassCardData data) return data;
                source = source.Parent;
            }
            return null;
        }

        private void CtxCopyCourseCode_Click(object? sender, EventArgs e)
        {
            var data = GetClassCardDataFromContext(sender);
            if (data is not null)
                Clipboard.SetText(data.CourseCode);
        }

        private void CtxCopyProfessor_Click(object? sender, EventArgs e)
        {
            var data = GetClassCardDataFromContext(sender);
            if (data is not null)
                Clipboard.SetText(data.Professor);
        }

        private void CtxCopyRoom_Click(object? sender, EventArgs e)
        {
            var data = GetClassCardDataFromContext(sender);
            if (data is not null)
                Clipboard.SetText(data.Room);
        }

        // ===========================================
        // Logout
        // ===========================================

        private void PerformLogout()
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            AuthService.Logout();
            var loginForm = new LoginForm();
            loginForm.Show();
            loginForm.FormClosed += (_, _) => Close();
            Hide();
        }

        // ===========================================
        // Paint Events
        // ===========================================

        private void StatCard_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;
            using var brush = new SolidBrush(Color.FromArgb(122, 26, 61));
            e.Graphics.FillRectangle(brush, 0, 0, panel.Width, 4);
        }

        private void ClassCard_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;
            using var brush = new SolidBrush(Color.FromArgb(122, 26, 61));
            e.Graphics.FillRectangle(brush, 0, 0, 4, panel.Height);
        }
    }

    internal record ClassCardData(string CourseCode, string Professor, string Room);
}
