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

        // Course offerings page state
        private List<JsonElement> _coAllCourses = [];
        private List<JsonElement> _coFilteredCourses = [];
        private JsonElement? _coSelectedCourse;
        private List<JsonElement> _coOfferings = [];
        private string _coSortBy = "section";
        private bool _coPageLoaded;

        // Change password page state
        private int _cpStep = 1;
        private string? _cpStoredOldPassword;
        private string? _cpStoredNewPassword;
        private DateTime? _cpOtpExpiresAt;
        private System.Windows.Forms.Timer? _cpTimer;

        public Form1()
        {
            InitializeComponent();
            LoadSidebarLogo();
            PopulateStudentInfo();
            PositionLogoutButton();
            _activeSidebarButton = btnDashboard;
            Load += async (_, _) => await LoadDashboardDataAsync();

            // Wire course offerings page events
            coSearchTextBox.TextChanged += CoSearchTextBox_TextChanged;
            coSearchTextBox.GotFocus += CoSearchTextBox_GotFocus;
            coSearchButton.Click += CoSearchButton_Click;
            coDropdownListBox.SelectedIndexChanged += CoDropdownListBox_SelectedIndexChanged;
            coDataGridView.ColumnHeaderMouseClick += CoDataGridView_ColumnHeaderMouseClick;
            coDataGridView.CellFormatting += CoDataGridView_CellFormatting;
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
                if (ud.TryGetProperty("firstName", out var fn))
                    firstName = fn.ToString();
                if (ud.TryGetProperty("lastName", out var ln))
                    lastName = ln.ToString();
                if (ud.TryGetProperty("studentIdNumber", out var sid))
                    visibleId = sid.ToString();
                else if (ud.TryGetProperty("userId", out var uid))
                    visibleId = uid.ToString();
            }

            var fullName = $"{firstName} {lastName}".Trim();
            var yearName = session.Get<string>("currentAcademicYearName") ?? "---";
            var termName = session.Get<string>("currentTermName") ?? "---";

            sidebarNameLabel.Text = fullName;
            sidebarIdLabel.Text = visibleId;
            welcomeNameLabel.Text = fullName;
            welcomeDetailsLabel.Text =
                $"Student ID: {visibleId}  |  Academic Year: {yearName}  |  Term: {termName}";
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
                DayOfWeek.Sunday => "SU",
                DayOfWeek.Monday => "M",
                DayOfWeek.Tuesday => "T",
                DayOfWeek.Wednesday => "W",
                DayOfWeek.Thursday => "TH",
                DayOfWeek.Friday => "F",
                DayOfWeek.Saturday => "S",
                _ => "",
            };

            // Each item has a "schedule" array, each entry has a "day" array of {code: "M"} objects
            var todayClasses = new List<(JsonElement course, JsonElement sched)>();

            foreach (var course in schedule.EnumerateArray())
            {
                if (
                    !course.TryGetProperty("schedule", out var schedArr)
                    || schedArr.ValueKind != JsonValueKind.Array
                )
                    continue;

                foreach (var sched in schedArr.EnumerateArray())
                {
                    if (
                        !sched.TryGetProperty("day", out var dayArr)
                        || dayArr.ValueKind != JsonValueKind.Array
                    )
                        continue;

                    foreach (var d in dayArr.EnumerateArray())
                    {
                        if (
                            d.TryGetProperty("code", out var codeProp)
                            && codeProp
                                .GetString()
                                ?.Equals(todayCode, StringComparison.OrdinalIgnoreCase) == true
                        )
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
            todayClasses.Sort(
                (a, b) =>
                {
                    var ta = TryGetString(a.sched, "timeFrom") ?? "";
                    var tb = TryGetString(b.sched, "timeFrom") ?? "";
                    return ParseTimeMinutes(ta).CompareTo(ParseTimeMinutes(tb));
                }
            );

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
            if (string.IsNullOrEmpty(timeStr))
                return 0;
            var match = System.Text.RegularExpressions.Regex.Match(
                timeStr,
                @"(\d{1,2}):(\d{2})\s*(AM|PM)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
            if (!match.Success)
                return 0;
            var hours = int.Parse(match.Groups[1].Value);
            var minutes = int.Parse(match.Groups[2].Value);
            var period = match.Groups[3].Value.ToUpper();
            if (period == "PM" && hours != 12)
                hours += 12;
            else if (period == "AM" && hours == 12)
                hours = 0;
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
                AutoSize = true,
            };
            codeLabel.ContextMenuStrip = classContextMenu;

            var nameLabel = new Label
            {
                Text = courseName,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(14, 34),
                AutoSize = true,
            };
            nameLabel.ContextMenuStrip = classContextMenu;

            var timeLabel = new Label
            {
                Text = time,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(400, 10),
                AutoSize = true,
            };
            timeLabel.ContextMenuStrip = classContextMenu;

            var roomLabel = new Label
            {
                Text = $"Room: {room}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(400, 34),
                AutoSize = true,
            };
            roomLabel.ContextMenuStrip = classContextMenu;

            var profLabel = new Label
            {
                Text = professor,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(120, 120, 120),
                Location = new Point(600, 10),
                AutoSize = true,
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
            if (
                data.TryGetProperty("studentEnrollments", out var enrollments)
                && enrollments.ValueKind == JsonValueKind.Array
            )
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
                    if (
                        current.TryGetProperty("term", out var termProp)
                        && termProp.ValueKind == JsonValueKind.String
                    )
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

        private void BtnSchedule_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnSchedule);
            using var form = new ScheduleForm();
            form.ShowDialog(this);
            SetActiveSidebarButton(btnDashboard);
        }

        private void BtnGrades_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnGrades);
            using var form = new GradesForm();
            form.ShowDialog(this);
            SetActiveSidebarButton(btnDashboard);
        }

        private void BtnProfessors_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnProfessors);
            using var form = new ProfessorsForm();
            form.ShowDialog(this);
            SetActiveSidebarButton(btnDashboard);
        }

        private async void BtnCourseOfferings_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnCourseOfferings);
            ShowPage("courseOfferings");
            if (!_coPageLoaded)
                await CoLoadCoursesAsync();
        }

        private void BtnChangePassword_Click(object? sender, EventArgs e)
        {
            SetActiveSidebarButton(btnChangePassword);
            ShowPage("changePassword");
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

        private void MenuDashboard_Click(object? sender, EventArgs e) =>
            BtnDashboard_Click(sender, e);

        private void MenuSchedule_Click(object? sender, EventArgs e) =>
            BtnSchedule_Click(sender, e);

        private void MenuGrades_Click(object? sender, EventArgs e) => BtnGrades_Click(sender, e);

        private void MenuProfessors_Click(object? sender, EventArgs e) =>
            BtnProfessors_Click(sender, e);

        private void MenuCourseOfferings_Click(object? sender, EventArgs e) =>
            BtnCourseOfferings_Click(sender, e);

        private void MenuChangePassword_Click(object? sender, EventArgs e) =>
            BtnChangePassword_Click(sender, e);

        private void MenuAbout_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Wildcat One v1.0\nCIT-U Student Portal",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // ===========================================
        // ContextMenuStrip Handlers
        // ===========================================

        private ClassCardData? GetClassCardDataFromContext(object? sender)
        {
            if (sender is not ToolStripMenuItem menuItem)
                return null;
            if (menuItem.Owner is not ContextMenuStrip ctx)
                return null;
            var source = ctx.SourceControl;
            while (source is not null)
            {
                if (source.Tag is ClassCardData data)
                    return data;
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
            courseOfferingsPagePanel.Visible = page == "courseOfferings";
            changePasswordPagePanel.Visible = page == "changePassword";
        }

        // ===========================================
        // Course Offerings Page
        // ===========================================

        private async Task CoLoadCoursesAsync()
        {
            coLoadingLabel.Text = "Loading courses...";
            coLoadingLabel.Visible = true;
            coErrorLabel.Visible = false;
            coSearchTextBox.Enabled = false;
            coSearchButton.Enabled = false;

            try
            {
                var result = await CourseOfferingsService.LoadCoursesAsync();

                if (
                    result.Status == 200
                    && result.Data.TryGetProperty("items", out var items)
                    && items.ValueKind == JsonValueKind.Array
                )
                {
                    _coAllCourses = items.EnumerateArray().ToList();
                    _coFilteredCourses = new List<JsonElement>(_coAllCourses);
                    _coPageLoaded = true;
                }
                else
                {
                    coErrorLabel.Text = "Failed to load courses. Please try again.";
                    coErrorLabel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                coErrorLabel.Text = ex.Message;
                coErrorLabel.Visible = true;
            }
            finally
            {
                coLoadingLabel.Visible = false;
                coSearchTextBox.Enabled = true;
                coSearchButton.Enabled = true;
            }
        }

        private void CoSearchTextBox_GotFocus(object? sender, EventArgs e)
        {
            // When focusing with empty text, show all courses
            if (string.IsNullOrEmpty(coSearchTextBox.Text.Trim()) && _coAllCourses.Count > 0)
            {
                _coSelectedCourse = null;
                CoShowDropdown(_coAllCourses);
            }
        }

        private void CoSearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            _coSelectedCourse = null;
            var searchText = coSearchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                _coFilteredCourses = new List<JsonElement>(_coAllCourses);
                CoShowDropdown(_coAllCourses);
                return;
            }

            _coFilteredCourses = _coAllCourses
                .Where(c =>
                {
                    var code = TryGetString(c, "courseCode") ?? "";
                    var name = TryGetString(c, "courseName") ?? TryGetString(c, "name") ?? "";
                    return code.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        || name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                })
                .ToList();

            CoShowDropdown(_coFilteredCourses);
        }

        private void CoShowDropdown(List<JsonElement> courses)
        {
            coDropdownListBox.Items.Clear();

            if (courses.Count > 0)
            {
                foreach (var course in courses)
                {
                    var code = TryGetString(course, "courseCode") ?? "";
                    var name = TryGetString(course, "courseName") ?? TryGetString(course, "name") ?? "";
                    coDropdownListBox.Items.Add($"{code} - {name}");
                }

                var itemHeight = coDropdownListBox.ItemHeight;
                var visibleItems = Math.Min(courses.Count, 10);
                coDropdownListBox.Size = new Size(500, visibleItems * itemHeight + 4);
                coDropdownListBox.Visible = true;
                coDropdownListBox.BringToFront();
            }
            else
            {
                coDropdownListBox.Visible = false;
            }
        }

        private async void CoDropdownListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var idx = coDropdownListBox.SelectedIndex;
            if (idx < 0 || idx >= _coFilteredCourses.Count)
                return;

            _coSelectedCourse = _coFilteredCourses[idx];

            var code = TryGetString(_coSelectedCourse.Value, "courseCode") ?? "";
            var name =
                TryGetString(_coSelectedCourse.Value, "courseName")
                ?? TryGetString(_coSelectedCourse.Value, "name")
                ?? "";
            coSearchTextBox.TextChanged -= CoSearchTextBox_TextChanged;
            coSearchTextBox.Text = $"{code} - {name}";
            coSearchTextBox.TextChanged += CoSearchTextBox_TextChanged;

            coDropdownListBox.Visible = false;

            await CoSearchOfferingsAsync();
        }

        private async void CoSearchButton_Click(object? sender, EventArgs e)
        {
            if (_coSelectedCourse is null)
            {
                // Try to auto-select first match
                if (_coFilteredCourses.Count > 0)
                {
                    _coSelectedCourse = _coFilteredCourses[0];
                    var code = TryGetString(_coSelectedCourse.Value, "courseCode") ?? "";
                    var name =
                        TryGetString(_coSelectedCourse.Value, "courseName")
                        ?? TryGetString(_coSelectedCourse.Value, "name")
                        ?? "";
                    coSearchTextBox.TextChanged -= CoSearchTextBox_TextChanged;
                    coSearchTextBox.Text = $"{code} - {name}";
                    coSearchTextBox.TextChanged += CoSearchTextBox_TextChanged;
                    coDropdownListBox.Visible = false;
                }
                else
                {
                    coErrorLabel.Text = "Please select a course first.";
                    coErrorLabel.Visible = true;
                    return;
                }
            }

            await CoSearchOfferingsAsync();
        }

        private async Task CoSearchOfferingsAsync()
        {
            if (_coSelectedCourse is not JsonElement selectedCourse)
                return;

            coLoadingLabel.Text = "Searching offerings...";
            coLoadingLabel.Visible = true;
            coErrorLabel.Visible = false;
            coEmptyLabel.Visible = false;
            coTableContainer.Visible = false;
            coInfoPanel.Visible = false;
            coSearchButton.Enabled = false;

            try
            {
                var idCourse = selectedCourse.TryGetProperty("idCourse", out var idProp)
                    ? idProp.ToString()
                    : "";

                var result = await CourseOfferingsService.SearchOfferingsAsync(idCourse);

                if (result.Status == 200)
                {
                    // Try multiple response structures:
                    // 1. data.items.courseOfferings (array)
                    // 2. data.items (array directly)
                    // 3. data.items (object with nested arrays)
                    List<JsonElement>? offeringsList = null;

                    if (result.Data.TryGetProperty("items", out var items))
                    {
                        if (
                            items.TryGetProperty("courseOfferings", out var offerings)
                            && offerings.ValueKind == JsonValueKind.Array
                        )
                        {
                            offeringsList = offerings.EnumerateArray().ToList();
                        }
                        else if (items.ValueKind == JsonValueKind.Array)
                        {
                            offeringsList = items.EnumerateArray().ToList();
                        }
                        else if (items.ValueKind == JsonValueKind.Object)
                        {
                            // Try to find any array property inside items
                            foreach (var prop in items.EnumerateObject())
                            {
                                if (prop.Value.ValueKind == JsonValueKind.Array)
                                {
                                    offeringsList = prop.Value.EnumerateArray().ToList();
                                    break;
                                }
                            }
                        }
                    }
                    else if (result.Data.ValueKind == JsonValueKind.Array)
                    {
                        offeringsList = result.Data.EnumerateArray().ToList();
                    }

                    if (offeringsList is not null)
                    {
                        _coOfferings = offeringsList;

                        var code = TryGetString(selectedCourse, "courseCode") ?? "";
                        var name =
                            TryGetString(selectedCourse, "courseName")
                            ?? TryGetString(selectedCourse, "name")
                            ?? "";
                        coInfoLabel.Text = $"Course Code: {code}  |  Course Name: {name}";
                        coInfoPanel.Visible = true;

                        if (_coOfferings.Count > 0)
                        {
                            CoPopulateOfferingsTable();
                            coTableContainer.Visible = true;
                            coEmptyLabel.Visible = false;
                        }
                        else
                        {
                            coTableContainer.Visible = false;
                            coEmptyLabel.Visible = true;
                        }
                    }
                    else
                    {
                        // Show response structure for debugging
                        var keys = new List<string>();
                        if (result.Data.ValueKind == JsonValueKind.Object)
                        {
                            foreach (var prop in result.Data.EnumerateObject())
                                keys.Add($"{prop.Name}({prop.Value.ValueKind})");
                        }
                        var debugInfo = keys.Count > 0
                            ? $"Keys: {string.Join(", ", keys)}"
                            : $"Type: {result.Data.ValueKind}";
                        coErrorLabel.Text =
                            $"Unexpected response format. {debugInfo}";
                        coErrorLabel.Visible = true;
                    }
                }
                else
                {
                    var msg = "Failed to load offerings.";
                    if (
                        result.Data.ValueKind == JsonValueKind.Object
                        && result.Data.TryGetProperty("message", out var msgProp)
                    )
                        msg = msgProp.GetString() ?? msg;
                    coErrorLabel.Text = $"{msg} (Status: {result.Status})";
                    coErrorLabel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                coErrorLabel.Text = ex.Message;
                coErrorLabel.Visible = true;
            }
            finally
            {
                coLoadingLabel.Visible = false;
                coSearchButton.Enabled = true;
            }
        }

        private void CoPopulateOfferingsTable()
        {
            coDataGridView.Rows.Clear();

            // Sort offerings
            var sorted = CoSortOfferings(_coOfferings, _coSortBy);

            foreach (var offering in sorted)
            {
                // Section
                var section = TryGetString(offering, "courseSection") ?? "N/A";

                // Schedule: array of schedule entries
                var scheduleText = "";
                if (
                    offering.TryGetProperty("schedule", out var schedArr)
                    && schedArr.ValueKind == JsonValueKind.Array
                )
                {
                    var lines = new List<string>();
                    foreach (var sched in schedArr.EnumerateArray())
                    {
                        // day is a simple string like "W" or "MTH" (padded with \u00A0)
                        var day = (TryGetString(sched, "day") ?? "TBA").Trim().Replace("\u00A0", "").Trim();
                        // time is a combined string like "11:30 AM - 01:30 PM"
                        var time = (TryGetString(sched, "time") ?? "TBA").Trim().Replace("\u00A0", "").Trim();
                        // roomNo is the clean room identifier
                        var room = (TryGetString(sched, "roomNo") ?? TryGetString(sched, "room") ?? "TBA").Trim().Replace("\u00A0", "").Trim();

                        // Check if lab
                        var isLab = false;
                        if (sched.TryGetProperty("lab", out var labProp))
                        {
                            if (labProp.ValueKind == JsonValueKind.True)
                                isLab = true;
                            else if (labProp.ValueKind == JsonValueKind.String)
                                isLab = labProp.GetString()?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
                        }
                        var labIndicator = isLab ? " (Lab)" : "";

                        var line = $"{day}{labIndicator} {time}";
                        if (!string.IsNullOrEmpty(room) && room != "TBA")
                            line += $"\n📍 {room}";
                        lines.Add(line);
                    }
                    scheduleText = string.Join("\n", lines);
                }

                // Slots: assessed + enrolled vs maxStudents
                int assessed = 0, enrolled = 0, maxStudents = 0;
                if (offering.TryGetProperty("assessed", out var assessedProp))
                    int.TryParse(assessedProp.ToString(), out assessed);
                if (offering.TryGetProperty("enrolled", out var enrolledProp))
                    int.TryParse(enrolledProp.ToString(), out enrolled);
                if (offering.TryGetProperty("maxStudents", out var maxProp))
                    int.TryParse(maxProp.ToString(), out maxStudents);
                var occupied = assessed + enrolled;
                var slotsText = $"{occupied} / {maxStudents}";

                // Faculty
                var faculty = TryGetString(offering, "faculty") ?? "";
                var coFaculty = TryGetString(offering, "coFaculty") ?? "";
                var facultyText = faculty;
                if (!string.IsNullOrEmpty(coFaculty))
                    facultyText += $"\n{coFaculty}";
                if (string.IsNullOrEmpty(facultyText))
                    facultyText = "TBA";

                // Mode
                var mode = TryGetString(offering, "modeOfDelivery") ?? "N/A";

                // Status
                var isClosed = TryGetString(offering, "isClosed") ?? "N";
                var reserved = TryGetString(offering, "reserved") ?? "N";
                string status;
                if (isClosed.Equals("Y", StringComparison.OrdinalIgnoreCase)
                    || reserved.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    status = "Closed";
                else if (occupied >= maxStudents && maxStudents > 0)
                    status = "Full";
                else
                    status = "Open";

                // Remarks
                var remarks = TryGetString(offering, "remarks") ?? "";

                coDataGridView.Rows.Add(
                    section,
                    scheduleText,
                    slotsText,
                    facultyText,
                    mode,
                    status,
                    remarks
                );
            }
        }

        private static List<JsonElement> CoSortOfferings(List<JsonElement> offerings, string sortBy)
        {
            return sortBy switch
            {
                "section" => offerings
                    .OrderBy(o => TryGetString(o, "courseSection") ?? "")
                    .ToList(),
                "slots" =>
                    offerings
                        .OrderByDescending(o =>
                        {
                            int.TryParse(
                                o.TryGetProperty("assessed", out var a)
                                    ? a.ToString()
                                    : "0",
                                out var assessed
                            );
                            int.TryParse(
                                o.TryGetProperty("enrolled", out var e)
                                    ? e.ToString()
                                    : "0",
                                out var enrolled
                            );
                            return assessed + enrolled;
                        })
                        .ToList(),
                "faculty" => offerings
                    .OrderBy(o => TryGetString(o, "faculty") ?? "ZZZ")
                    .ToList(),
                "status" =>
                    offerings
                        .OrderBy(o =>
                        {
                            var isClosed = TryGetString(o, "isClosed") ?? "N";
                            var reserved = TryGetString(o, "reserved") ?? "N";
                            if (
                                isClosed.Equals("Y", StringComparison.OrdinalIgnoreCase)
                                || reserved.Equals("Y", StringComparison.OrdinalIgnoreCase)
                            )
                                return 2;
                            int.TryParse(
                                o.TryGetProperty("assessed", out var a)
                                    ? a.ToString()
                                    : "0",
                                out var assessed
                            );
                            int.TryParse(
                                o.TryGetProperty("enrolled", out var e)
                                    ? e.ToString()
                                    : "0",
                                out var enrolled
                            );
                            int.TryParse(
                                o.TryGetProperty("maxStudents", out var m)
                                    ? m.ToString()
                                    : "0",
                                out var max
                            );
                            return (assessed + enrolled >= max && max > 0) ? 1 : 0;
                        })
                        .ToList(),
                _ => offerings,
            };
        }

        private void CoDataGridView_ColumnHeaderMouseClick(
            object? sender,
            DataGridViewCellMouseEventArgs e
        )
        {
            var colName = coDataGridView.Columns[e.ColumnIndex].Name;
            _coSortBy = colName switch
            {
                "Section" => "section",
                "Slots" => "slots",
                "Faculty" => "faculty",
                "Status" => "status",
                _ => _coSortBy,
            };

            if (_coOfferings.Count > 0)
                CoPopulateOfferingsTable();
        }

        private void CoDataGridView_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e
        )
        {
            // Status column is index 5
            if (e.ColumnIndex != 5)
                return;
            if (e.Value is not string statusStr || string.IsNullOrEmpty(statusStr))
                return;

            Color statusColor = statusStr switch
            {
                "Open" => Color.FromArgb(39, 174, 96),
                "Full" => Color.FromArgb(243, 156, 18),
                "Closed" => Color.FromArgb(231, 76, 60),
                _ => Color.FromArgb(52, 73, 94),
            };

            e.CellStyle!.ForeColor = statusColor;
            e.CellStyle.SelectionForeColor = statusColor;
            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        // ===========================================
        // Change Password Page
        // ===========================================

        private void CpShowError(string message)
        {
            cpSuccessPanel.Visible = false;
            cpSuccessPanel.Size = new Size(cpSuccessPanel.Width, 0);
            cpErrorLabel.Text = message;
            cpErrorPanel.Size = new Size(cpErrorPanel.Width, 48);
            cpErrorPanel.Visible = true;
        }

        private void CpShowSuccess(string message)
        {
            cpErrorPanel.Visible = false;
            cpErrorPanel.Size = new Size(cpErrorPanel.Width, 0);
            cpSuccessLabel.Text = message;
            cpSuccessPanel.Size = new Size(cpSuccessPanel.Width, 48);
            cpSuccessPanel.Visible = true;
        }

        private void CpHideMessages()
        {
            cpErrorPanel.Visible = false;
            cpErrorPanel.Size = new Size(cpErrorPanel.Width, 0);
            cpSuccessPanel.Visible = false;
            cpSuccessPanel.Size = new Size(cpSuccessPanel.Width, 0);
        }

        private async void CpRequestOtp_Click(object? sender, EventArgs e)
        {
            CpHideMessages();

            var oldPassword = cpOldPasswordTextBox.Text.Trim();
            var newPassword = cpNewPasswordTextBox.Text.Trim();
            var confirmPassword = cpConfirmPasswordTextBox.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(oldPassword))
            {
                CpShowError("Current password is required.");
                return;
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                CpShowError("New password is required.");
                return;
            }
            if (newPassword.Length < 6)
            {
                CpShowError("New password must be at least 6 characters.");
                return;
            }
            if (newPassword == oldPassword)
            {
                CpShowError("New password must be different from current password.");
                return;
            }
            if (string.IsNullOrEmpty(confirmPassword))
            {
                CpShowError("Please confirm your new password.");
                return;
            }
            if (confirmPassword != newPassword)
            {
                CpShowError("Passwords do not match.");
                return;
            }

            // Store passwords for step 2
            _cpStoredOldPassword = oldPassword;
            _cpStoredNewPassword = newPassword;

            cpRequestOtpButton.Enabled = false;
            cpRequestOtpButton.Text = "Sending OTP...";

            try
            {
                var result = await ChangePasswordService.RequestOtpAsync(oldPassword, newPassword);

                if (result.Status == 200)
                {
                    CpShowSuccess("OTP has been sent to your registered email.");
                    CpSwitchToStep2();
                }
                else if (result.Status == 429)
                {
                    CpShowError("Too many requests. Please wait 5 minutes.");
                }
                else if (result.Status == 400)
                {
                    CpShowError("Invalid password.");
                }
                else if (result.Status == 401)
                {
                    CpShowError("Current password is incorrect.");
                }
                else
                {
                    var msg = "An error occurred. Please try again.";
                    if (
                        result.Data.ValueKind == JsonValueKind.Object
                        && result.Data.TryGetProperty("message", out var msgProp)
                    )
                        msg = msgProp.GetString() ?? msg;
                    CpShowError(msg);
                }
            }
            catch (Exception ex)
            {
                CpShowError(ex.Message);
            }
            finally
            {
                cpRequestOtpButton.Enabled = true;
                cpRequestOtpButton.Text = "Request OTP";
            }
        }

        private void CpSwitchToStep2()
        {
            _cpStep = 2;
            cpStep1Panel.Visible = false;
            cpStep2Panel.Visible = true;
            cpOtpTextBox.Text = "";
            cpOtpTextBox.Enabled = true;
            cpSubmitButton.Enabled = true;

            // Start 5-minute timer
            _cpOtpExpiresAt = DateTime.Now.AddMinutes(5);
            _cpTimer?.Stop();
            _cpTimer?.Dispose();
            _cpTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _cpTimer.Tick += CpTimer_Tick;
            _cpTimer.Start();
            CpTimer_Tick(null, EventArgs.Empty); // Update immediately
        }

        private void CpTimer_Tick(object? sender, EventArgs e)
        {
            if (_cpOtpExpiresAt is not DateTime expiresAt)
            {
                _cpTimer?.Stop();
                return;
            }

            var remaining = expiresAt - DateTime.Now;

            if (remaining.TotalSeconds <= 0)
            {
                cpOtpTimerLabel.Text = "OTP expired";
                cpOtpTimerLabel.ForeColor = Color.FromArgb(231, 76, 60);
                cpOtpTextBox.Enabled = false;
                cpSubmitButton.Enabled = false;
                _cpTimer?.Stop();
                return;
            }

            var minutes = (int)remaining.TotalMinutes;
            var seconds = remaining.Seconds;
            cpOtpTimerLabel.Text = $"OTP expires in {minutes}:{seconds:D2}";

            if (remaining.TotalSeconds < 60)
                cpOtpTimerLabel.ForeColor = Color.FromArgb(231, 76, 60);
            else
                cpOtpTimerLabel.ForeColor = _maroon;
        }

        private async void CpSubmitPasswordChange_Click(object? sender, EventArgs e)
        {
            CpHideMessages();

            var otp = cpOtpTextBox.Text.Trim();

            if (string.IsNullOrEmpty(otp))
            {
                CpShowError("OTP is required.");
                return;
            }
            if (!Regex.IsMatch(otp, @"^\d{6}$"))
            {
                CpShowError("OTP must be exactly 6 digits.");
                return;
            }
            if (_cpStoredOldPassword is null || _cpStoredNewPassword is null)
            {
                CpShowError("Session expired. Please start over.");
                return;
            }

            cpSubmitButton.Enabled = false;
            cpSubmitButton.Text = "Changing Password...";

            try
            {
                var result = await ChangePasswordService.SubmitPasswordChangeAsync(
                    otp,
                    _cpStoredOldPassword,
                    _cpStoredNewPassword
                );

                if (result.Status == 200)
                {
                    CpShowSuccess("Password changed successfully!");
                    _cpTimer?.Stop();

                    await Task.Delay(2000);

                    if (!IsDisposed)
                        CpResetForm();
                }
                else
                {
                    var msg = "Failed to change password. Please try again.";
                    if (
                        result.Data.ValueKind == JsonValueKind.Object
                        && result.Data.TryGetProperty("message", out var msgProp)
                    )
                        msg = msgProp.GetString() ?? msg;
                    CpShowError(msg);
                }
            }
            catch (Exception ex)
            {
                CpShowError(ex.Message);
            }
            finally
            {
                if (!IsDisposed)
                {
                    cpSubmitButton.Enabled = true;
                    cpSubmitButton.Text = "Change Password";
                }
            }
        }

        private void CpStartOver_Click(object? sender, EventArgs e)
        {
            CpResetForm();
        }

        private void CpResetForm()
        {
            _cpStep = 1;
            _cpStoredOldPassword = null;
            _cpStoredNewPassword = null;
            _cpOtpExpiresAt = null;
            _cpTimer?.Stop();

            cpOldPasswordTextBox.Text = "";
            cpNewPasswordTextBox.Text = "";
            cpConfirmPasswordTextBox.Text = "";
            cpOtpTextBox.Text = "";

            cpOldPasswordTextBox.UseSystemPasswordChar = true;
            cpNewPasswordTextBox.UseSystemPasswordChar = true;
            cpConfirmPasswordTextBox.UseSystemPasswordChar = true;
            cpToggleOldPassword.Text = "SHOW";
            cpToggleNewPassword.Text = "SHOW";
            cpToggleConfirmPassword.Text = "SHOW";

            CpHideMessages();

            cpStep1Panel.Visible = true;
            cpStep2Panel.Visible = false;

            cpOtpTextBox.Enabled = true;
            cpSubmitButton.Enabled = true;
            cpSubmitButton.Text = "Change Password";
            cpRequestOtpButton.Enabled = true;
            cpRequestOtpButton.Text = "Request OTP";
        }

        private void CpToggleOldPassword_Click(object? sender, EventArgs e)
        {
            cpOldPasswordTextBox.UseSystemPasswordChar =
                !cpOldPasswordTextBox.UseSystemPasswordChar;
            cpToggleOldPassword.Text = cpOldPasswordTextBox.UseSystemPasswordChar ? "SHOW" : "HIDE";
        }

        private void CpToggleNewPassword_Click(object? sender, EventArgs e)
        {
            cpNewPasswordTextBox.UseSystemPasswordChar =
                !cpNewPasswordTextBox.UseSystemPasswordChar;
            cpToggleNewPassword.Text = cpNewPasswordTextBox.UseSystemPasswordChar ? "SHOW" : "HIDE";
        }

        private void CpToggleConfirmPassword_Click(object? sender, EventArgs e)
        {
            cpConfirmPasswordTextBox.UseSystemPasswordChar =
                !cpConfirmPasswordTextBox.UseSystemPasswordChar;
            cpToggleConfirmPassword.Text = cpConfirmPasswordTextBox.UseSystemPasswordChar
                ? "SHOW"
                : "HIDE";
        }

        private void CpOtpTextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void CpOtpTextBox_TextChanged(object? sender, EventArgs e)
        {
            var text = cpOtpTextBox.Text;
            var digitsOnly = new string(text.Where(char.IsDigit).ToArray());
            if (digitsOnly.Length > 6)
                digitsOnly = digitsOnly[..6];
            if (text != digitsOnly)
            {
                cpOtpTextBox.Text = digitsOnly;
                cpOtpTextBox.SelectionStart = digitsOnly.Length;
            }
        }

        private void CpErrorPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel)
                return;
            using var brush = new SolidBrush(Color.FromArgb(231, 76, 60));
            e.Graphics.FillRectangle(brush, 0, 0, 4, panel.Height);
        }

        private void CpSuccessPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel)
                return;
            using var brush = new SolidBrush(Color.FromArgb(39, 174, 96));
            e.Graphics.FillRectangle(brush, 0, 0, 4, panel.Height);
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
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

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
            if (sender is not Panel panel)
                return;
            using var brush = new SolidBrush(Color.FromArgb(122, 26, 61));
            e.Graphics.FillRectangle(brush, 0, 0, panel.Width, 4);
        }

        private void ClassCard_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel)
                return;
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
        string RoomName
    );

    internal class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.UserPaint
                    | ControlStyles.OptimizedDoubleBuffer,
                true
            );
        }
    }
}
