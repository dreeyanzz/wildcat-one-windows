using System.Text.Json;
using System.Text.RegularExpressions;
using wildcat_one_windows.Services;

namespace wildcat_one_windows
{
    public partial class Form1 : Form
    {
        private readonly Color _maroon = Color.FromArgb(122, 26, 61);
        private readonly Color _maroonLight = Color.FromArgb(142, 46, 81);
        private Button _activeSidebarButton = null!;
        private JsonElement? _scheduleData;

        // Schedule page state
        private List<SemesterOption> _semesterOptions = [];
        private JsonElement? _scheduleCourses;
        private List<ScheduleBlock> _scheduleBlocks = [];
        private bool _schedulePageLoaded;
        private readonly ToolTip _gridTooltip = new() { InitialDelay = 200, ReshowDelay = 100 };
        private ScheduleBlock? _hoveredBlock;

        public Form1()
        {
            InitializeComponent();
            LoadSidebarLogo();
            PopulateStudentInfo();
            PositionLogoutButton();
            _activeSidebarButton = btnDashboard;
            Load += async (_, _) => await LoadDashboardDataAsync();

            // Wire schedule page events
            semesterComboBox.SelectedIndexChanged += SemesterComboBox_SelectedIndexChanged;
            scheduleGridPanel.Paint += ScheduleGridPanel_Paint;
            scheduleGridPanel.MouseMove += ScheduleGridPanel_MouseMove;
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
            ShowPage("dashboard");
        }

        private async void BtnSchedule_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnSchedule);
            ShowPage("schedule");
            if (!_schedulePageLoaded)
                await LoadSchedulePageAsync();
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
        // Page Switching
        // ===========================================

        private void ShowPage(string page)
        {
            dashboardPanel.Visible = page == "dashboard";
            schedulePagePanel.Visible = page == "schedule";
        }

        // ===========================================
        // Schedule Page
        // ===========================================

        private async Task LoadSchedulePageAsync()
        {
            semesterComboBox.Items.Clear();
            semesterComboBox.Items.Add("Loading semesters...");
            semesterComboBox.SelectedIndex = 0;
            semesterComboBox.Enabled = false;

            try
            {
                _semesterOptions = await ScheduleService.LoadAllSemesterOptionsAsync();

                semesterComboBox.Items.Clear();
                foreach (var opt in _semesterOptions)
                    semesterComboBox.Items.Add(opt.DisplayText);

                semesterComboBox.Enabled = true;

                if (_semesterOptions.Count > 0)
                {
                    // Pre-select the current semester if found
                    var session = SessionManager.Instance;
                    var currentYearId = session.Get<string>("currentAcademicYearId");
                    var currentTermId = session.Get<string>("currentTermId");
                    var selectedIdx = 0;

                    if (currentYearId is not null && currentTermId is not null)
                    {
                        for (int i = 0; i < _semesterOptions.Count; i++)
                        {
                            if (_semesterOptions[i].YearId == currentYearId && _semesterOptions[i].TermId == currentTermId)
                            {
                                selectedIdx = i;
                                break;
                            }
                        }
                    }

                    semesterComboBox.SelectedIndex = selectedIdx;
                }
                _schedulePageLoaded = true;
            }
            catch
            {
                semesterComboBox.Items.Clear();
                semesterComboBox.Items.Add("Failed to load semesters");
                semesterComboBox.SelectedIndex = 0;
            }
        }

        private async void SemesterComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!semesterComboBox.Enabled || _semesterOptions.Count == 0) return;
            var idx = semesterComboBox.SelectedIndex;
            if (idx < 0 || idx >= _semesterOptions.Count) return;

            var opt = _semesterOptions[idx];
            semesterInfoLabel.Text = $"Academic Year: {opt.YearName}  |  Semester: {opt.TermName}";

            var studentId = SessionManager.Instance.UserData?.GetProperty("studentId").ToString();
            if (studentId is null) return;

            try
            {
                _scheduleCourses = await ScheduleService.FetchScheduleAsync(studentId, opt.YearId, opt.TermId);
            }
            catch
            {
                _scheduleCourses = null;
            }

            _scheduleBlocks.Clear();
            scheduleGridPanel.Invalidate();
        }

        // ===========================================
        // Schedule Grid Painting
        // ===========================================

        private static readonly Dictionary<string, int> DayCodeMap = new()
        {
            ["M"] = 0, ["T"] = 1, ["W"] = 2, ["TH"] = 3, ["F"] = 4, ["S"] = 5, ["SU"] = 6
        };

        private static readonly string[] DayNames = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

        private static readonly string[] TimeLabels =
        [
            "7:30 AM", "8:00 AM", "8:30 AM", "9:00 AM", "9:30 AM", "10:00 AM", "10:30 AM", "11:00 AM", "11:30 AM",
            "12:00 PM", "12:30 PM", "1:00 PM", "1:30 PM", "2:00 PM", "2:30 PM", "3:00 PM", "3:30 PM", "4:00 PM",
            "4:30 PM", "5:00 PM", "5:30 PM", "6:00 PM", "6:30 PM", "7:00 PM", "7:30 PM", "8:00 PM", "8:30 PM"
        ];

        private const int TimeLabelWidth = 80;
        private const int HeaderHeight = 36;
        private const int SlotHeight = 40;
        private const int TotalSlots = 27;

        private static readonly Color[] PastelColors =
        [
            Color.FromArgb(255, 179, 186), Color.FromArgb(255, 223, 186), Color.FromArgb(255, 255, 186),
            Color.FromArgb(186, 255, 201), Color.FromArgb(186, 225, 255), Color.FromArgb(218, 186, 255),
            Color.FromArgb(255, 186, 255), Color.FromArgb(186, 255, 255), Color.FromArgb(255, 204, 204),
            Color.FromArgb(255, 229, 204), Color.FromArgb(255, 255, 204), Color.FromArgb(204, 255, 204),
            Color.FromArgb(204, 229, 255), Color.FromArgb(229, 204, 255), Color.FromArgb(255, 204, 229),
            Color.FromArgb(204, 255, 229), Color.FromArgb(255, 218, 185), Color.FromArgb(230, 230, 250),
            Color.FromArgb(255, 228, 225), Color.FromArgb(240, 255, 240), Color.FromArgb(245, 245, 220),
            Color.FromArgb(255, 250, 205), Color.FromArgb(250, 250, 210), Color.FromArgb(255, 239, 213),
            Color.FromArgb(253, 245, 230), Color.FromArgb(255, 245, 238), Color.FromArgb(245, 255, 250),
            Color.FromArgb(240, 248, 255), Color.FromArgb(248, 248, 255), Color.FromArgb(255, 240, 245),
            Color.FromArgb(255, 228, 196), Color.FromArgb(255, 235, 205), Color.FromArgb(255, 248, 220),
            Color.FromArgb(224, 255, 255), Color.FromArgb(230, 230, 250), Color.FromArgb(255, 240, 245),
            Color.FromArgb(250, 235, 215), Color.FromArgb(245, 222, 179), Color.FromArgb(255, 222, 173),
            Color.FromArgb(255, 218, 185), Color.FromArgb(238, 232, 170), Color.FromArgb(152, 251, 152),
            Color.FromArgb(175, 238, 238), Color.FromArgb(176, 224, 230), Color.FromArgb(173, 216, 230),
            Color.FromArgb(135, 206, 250), Color.FromArgb(176, 196, 222), Color.FromArgb(216, 191, 216),
            Color.FromArgb(221, 160, 221), Color.FromArgb(238, 130, 238), Color.FromArgb(255, 182, 193),
            Color.FromArgb(255, 192, 203), Color.FromArgb(219, 112, 147), Color.FromArgb(255, 160, 122),
            Color.FromArgb(250, 128, 114), Color.FromArgb(233, 150, 122), Color.FromArgb(240, 128, 128),
            Color.FromArgb(205, 92, 92), Color.FromArgb(188, 143, 143), Color.FromArgb(210, 180, 140),
            Color.FromArgb(222, 184, 135), Color.FromArgb(244, 164, 96), Color.FromArgb(218, 165, 32),
            Color.FromArgb(189, 183, 107), Color.FromArgb(144, 238, 144), Color.FromArgb(143, 188, 143),
            Color.FromArgb(102, 205, 170), Color.FromArgb(127, 255, 212), Color.FromArgb(0, 250, 154),
            Color.FromArgb(72, 209, 204), Color.FromArgb(64, 224, 208), Color.FromArgb(0, 206, 209),
            Color.FromArgb(100, 149, 237), Color.FromArgb(106, 90, 205), Color.FromArgb(123, 104, 238),
            Color.FromArgb(147, 112, 219), Color.FromArgb(186, 85, 211), Color.FromArgb(218, 112, 214),
            Color.FromArgb(199, 21, 133), Color.FromArgb(219, 112, 147)
        ];

        private static int HashString(string str)
        {
            int hash = 0;
            foreach (char c in str)
            {
                hash = (hash << 5) - hash + c;
                hash &= hash;
            }
            return Math.Abs(hash);
        }

        private static Color GetCourseColor(string courseCode)
        {
            return PastelColors[HashString(courseCode) % PastelColors.Length];
        }

        private static int GetMinutesFrom7AM(string timeStr)
        {
            var totalMinutes = ParseTimeMinutes(timeStr);
            return totalMinutes - 7 * 60; // Subtract 7:00 AM (420 minutes)
        }

        private void ScheduleGridPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var dayColWidth = (scheduleGridPanel.Width - TimeLabelWidth) / 7;
            var gridWidth = scheduleGridPanel.Width;
            var gridBodyHeight = TotalSlots * SlotHeight;

            var newBlocks = new List<ScheduleBlock>();

            // --- Draw header background ---
            using (var headerBrush = new SolidBrush(Color.FromArgb(248, 249, 250)))
                g.FillRectangle(headerBrush, 0, 0, gridWidth, HeaderHeight);

            // --- Draw day column headers ---
            using var headerFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            using var headerTextBrush = new SolidBrush(Color.FromArgb(52, 73, 94));
            for (int i = 0; i < 7; i++)
            {
                var x = TimeLabelWidth + i * dayColWidth;
                var rect = new RectangleF(x, 0, dayColWidth, HeaderHeight);
                using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(DayNames[i], headerFont, headerTextBrush, rect, sf);
            }

            // --- Draw time labels ---
            using var timeFont = new Font("Segoe UI", 7.5F);
            using var timeBrush = new SolidBrush(Color.FromArgb(120, 120, 120));
            for (int i = 0; i < TotalSlots; i++)
            {
                var y = HeaderHeight + i * SlotHeight;
                var rect = new RectangleF(2, y, TimeLabelWidth - 6, SlotHeight);
                using var sf = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };
                g.DrawString(TimeLabels[i], timeFont, timeBrush, rect, sf);
            }

            // --- Draw grid lines ---
            using var gridPen = new Pen(Color.FromArgb(230, 230, 230), 1);

            // Horizontal lines
            for (int i = 0; i <= TotalSlots; i++)
            {
                var y = HeaderHeight + i * SlotHeight;
                g.DrawLine(gridPen, TimeLabelWidth, y, gridWidth, y);
            }

            // Vertical lines
            g.DrawLine(gridPen, TimeLabelWidth, 0, TimeLabelWidth, HeaderHeight + gridBodyHeight);
            for (int i = 0; i <= 7; i++)
            {
                var x = TimeLabelWidth + i * dayColWidth;
                g.DrawLine(gridPen, x, 0, x, HeaderHeight + gridBodyHeight);
            }

            // Header bottom border
            using var headerBorderPen = new Pen(Color.FromArgb(200, 200, 200), 1);
            g.DrawLine(headerBorderPen, 0, HeaderHeight, gridWidth, HeaderHeight);

            // --- Draw course blocks ---
            if (_scheduleCourses is JsonElement courses && courses.ValueKind == JsonValueKind.Array)
            {
                using var blockFont = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                using var blockDetailFont = new Font("Segoe UI", 7F);
                using var blockTextBrush = new SolidBrush(Color.FromArgb(40, 40, 40));

                foreach (var course in courses.EnumerateArray())
                {
                    var courseCode = TryGetString(course, "courseCode") ?? "";
                    var description = TryGetString(course, "description") ?? "";
                    var section = TryGetString(course, "section") ?? "";
                    var instructor = TryGetString(course, "instructor") ?? "";
                    var color = GetCourseColor(courseCode);

                    if (!course.TryGetProperty("schedule", out var schedArr) || schedArr.ValueKind != JsonValueKind.Array)
                        continue;

                    foreach (var sched in schedArr.EnumerateArray())
                    {
                        var timeFrom = TryGetString(sched, "timeFrom") ?? "";
                        var timeTo = TryGetString(sched, "timeTo") ?? "";
                        var roomName = TryGetString(sched, "roomName") ?? "";

                        if (!sched.TryGetProperty("day", out var dayArr) || dayArr.ValueKind != JsonValueKind.Array)
                            continue;

                        var startMins = GetMinutesFrom7AM(timeFrom);
                        var endMins = GetMinutesFrom7AM(timeTo);
                        var durationMins = endMins - startMins;
                        if (durationMins <= 0) continue;

                        // Position calculation: minutes from 7:30 AM
                        var minsFrom730 = startMins - 30;
                        if (minsFrom730 < 0) continue;

                        var yPos = HeaderHeight + (minsFrom730 / 30.0) * SlotHeight;
                        var blockHeight = (durationMins / 30.0) * SlotHeight - 6;
                        if (blockHeight < 10) blockHeight = 10;

                        foreach (var dayEl in dayArr.EnumerateArray())
                        {
                            var dayCode = dayEl.TryGetProperty("code", out var dc) ? dc.GetString() : null;
                            if (dayCode is null || !DayCodeMap.TryGetValue(dayCode, out var dayIndex))
                                continue;

                            var xPos = TimeLabelWidth + dayIndex * dayColWidth + 2;
                            var blockWidth = dayColWidth - 4;
                            var blockRect = new RectangleF(xPos, (float)yPos, blockWidth, (float)blockHeight);

                            // Fill block
                            using var blockBrush = new SolidBrush(color);
                            using var blockPath = CreateRoundedRect(blockRect, 4);
                            g.FillPath(blockBrush, blockPath);

                            // Draw border
                            using var borderColor = new SolidBrush(Color.FromArgb(40, 0, 0, 0));
                            using var borderPen2 = new Pen(borderColor, 1);
                            g.DrawPath(borderPen2, blockPath);

                            // Draw text inside block (NoClip prevents descender clipping on g, p, y, etc.)
                            using var textSf = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip };

                            var line1Rect = new RectangleF(xPos + 4, (float)yPos + 2, blockWidth - 8, 16);
                            g.DrawString(courseCode, blockFont, blockTextBrush, line1Rect, textSf);

                            if (blockHeight > 30)
                            {
                                var line2Rect = new RectangleF(xPos + 4, (float)yPos + 16, blockWidth - 8, 15);
                                g.DrawString(section, blockDetailFont, blockTextBrush, line2Rect, textSf);
                            }
                            if (blockHeight > 44)
                            {
                                var line3Rect = new RectangleF(xPos + 4, (float)yPos + 30, blockWidth - 8, 15);
                                g.DrawString(description, blockDetailFont, blockTextBrush, line3Rect, textSf);
                            }
                            if (blockHeight > 58)
                            {
                                var line4Rect = new RectangleF(xPos + 4, (float)yPos + 44, blockWidth - 8, 15);
                                g.DrawString(roomName, blockDetailFont, blockTextBrush, line4Rect, textSf);
                            }

                            // Store block for hit testing
                            newBlocks.Add(new ScheduleBlock(
                                blockRect, courseCode, description, section, instructor,
                                timeFrom, timeTo, roomName));
                        }
                    }
                }
            }

            _scheduleBlocks = newBlocks;
        }

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRect(RectangleF rect, float radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            var diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ScheduleGridPanel_MouseMove(object? sender, MouseEventArgs e)
        {
            ScheduleBlock? hit = null;
            foreach (var block in _scheduleBlocks)
            {
                if (block.Bounds.Contains(e.Location))
                {
                    hit = block;
                    break;
                }
            }

            if (hit == _hoveredBlock) return;
            _hoveredBlock = hit;

            if (hit is not null)
            {
                var tip = $"{hit.CourseCode} - {hit.Section}\n{hit.Description}\n{hit.Instructor}\n{hit.TimeFrom} - {hit.TimeTo}\nRoom: {hit.RoomName}";
                _gridTooltip.SetToolTip(scheduleGridPanel, tip);
                scheduleGridPanel.Cursor = Cursors.Hand;
            }
            else
            {
                _gridTooltip.SetToolTip(scheduleGridPanel, null);
                scheduleGridPanel.Cursor = Cursors.Default;
            }
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

    internal record ScheduleBlock(
        RectangleF Bounds,
        string CourseCode,
        string Description,
        string Section,
        string Instructor,
        string TimeFrom,
        string TimeTo,
        string RoomName);

    internal class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
        }
    }
}
