using System.Text.Json;
using wildcat_one_windows.Services;

namespace wildcat_one_windows
{
    public partial class ScheduleForm : Form
    {
        private List<SemesterOption> _semesterOptions = [];
        private JsonElement? _scheduleCourses;
        private List<ScheduleBlock> _scheduleBlocks = [];
        private readonly ToolTip _gridTooltip = new() { InitialDelay = 200, ReshowDelay = 100 };
        private ScheduleBlock? _hoveredBlock;

        public ScheduleForm()
        {
            InitializeComponent();
            semesterComboBox.SelectedIndexChanged += SemesterComboBox_SelectedIndexChanged;
            scheduleGridPanel.Paint += ScheduleGridPanel_Paint;
            scheduleGridPanel.MouseMove += ScheduleGridPanel_MouseMove;
            Load += async (_, _) => await LoadSchedulePageAsync();
        }

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
                    var session = SessionManager.Instance;
                    var currentYearId = session.Get<string>("currentAcademicYearId");
                    var currentTermId = session.Get<string>("currentTermId");
                    var selectedIdx = 0;

                    if (currentYearId is not null && currentTermId is not null)
                    {
                        for (int i = 0; i < _semesterOptions.Count; i++)
                        {
                            if (
                                _semesterOptions[i].YearId == currentYearId
                                && _semesterOptions[i].TermId == currentTermId
                            )
                            {
                                selectedIdx = i;
                                break;
                            }
                        }
                    }

                    semesterComboBox.SelectedIndex = selectedIdx;
                }
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
            if (!semesterComboBox.Enabled || _semesterOptions.Count == 0)
                return;
            var idx = semesterComboBox.SelectedIndex;
            if (idx < 0 || idx >= _semesterOptions.Count)
                return;

            var opt = _semesterOptions[idx];
            semesterInfoLabel.Text = $"Academic Year: {opt.YearName}  |  Semester: {opt.TermName}";

            var studentId = SessionManager.Instance.UserData?.GetProperty("studentId").ToString();
            if (studentId is null)
                return;

            try
            {
                _scheduleCourses = await ScheduleService.FetchScheduleAsync(
                    studentId,
                    opt.YearId,
                    opt.TermId
                );
            }
            catch
            {
                _scheduleCourses = null;
            }

            _scheduleBlocks.Clear();
            scheduleGridPanel.Invalidate();
        }

        // ===========================================
        // Schedule Grid Constants & Helpers
        // ===========================================

        private static readonly Dictionary<string, int> DayCodeMap = new()
        {
            ["M"] = 0,
            ["T"] = 1,
            ["W"] = 2,
            ["TH"] = 3,
            ["F"] = 4,
            ["S"] = 5,
            ["SU"] = 6,
        };

        private static readonly string[] DayNames =
        [
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday",
        ];

        private static readonly string[] TimeLabels =
        [
            "7:30 AM",
            "8:00 AM",
            "8:30 AM",
            "9:00 AM",
            "9:30 AM",
            "10:00 AM",
            "10:30 AM",
            "11:00 AM",
            "11:30 AM",
            "12:00 PM",
            "12:30 PM",
            "1:00 PM",
            "1:30 PM",
            "2:00 PM",
            "2:30 PM",
            "3:00 PM",
            "3:30 PM",
            "4:00 PM",
            "4:30 PM",
            "5:00 PM",
            "5:30 PM",
            "6:00 PM",
            "6:30 PM",
            "7:00 PM",
            "7:30 PM",
            "8:00 PM",
            "8:30 PM",
        ];

        private const int TimeLabelWidth = 80;
        private const int HeaderHeight = 36;
        private const int SlotHeight = 40;
        private const int TotalSlots = 27;

        private static readonly Color[] PastelColors =
        [
            Color.FromArgb(255, 179, 186),
            Color.FromArgb(255, 223, 186),
            Color.FromArgb(255, 255, 186),
            Color.FromArgb(186, 255, 201),
            Color.FromArgb(186, 225, 255),
            Color.FromArgb(218, 186, 255),
            Color.FromArgb(255, 186, 255),
            Color.FromArgb(186, 255, 255),
            Color.FromArgb(255, 204, 204),
            Color.FromArgb(255, 229, 204),
            Color.FromArgb(255, 255, 204),
            Color.FromArgb(204, 255, 204),
            Color.FromArgb(204, 229, 255),
            Color.FromArgb(229, 204, 255),
            Color.FromArgb(255, 204, 229),
            Color.FromArgb(204, 255, 229),
            Color.FromArgb(255, 218, 185),
            Color.FromArgb(230, 230, 250),
            Color.FromArgb(255, 228, 225),
            Color.FromArgb(240, 255, 240),
            Color.FromArgb(245, 245, 220),
            Color.FromArgb(255, 250, 205),
            Color.FromArgb(250, 250, 210),
            Color.FromArgb(255, 239, 213),
            Color.FromArgb(253, 245, 230),
            Color.FromArgb(255, 245, 238),
            Color.FromArgb(245, 255, 250),
            Color.FromArgb(240, 248, 255),
            Color.FromArgb(248, 248, 255),
            Color.FromArgb(255, 240, 245),
            Color.FromArgb(255, 228, 196),
            Color.FromArgb(255, 235, 205),
            Color.FromArgb(255, 248, 220),
            Color.FromArgb(224, 255, 255),
            Color.FromArgb(230, 230, 250),
            Color.FromArgb(255, 240, 245),
            Color.FromArgb(250, 235, 215),
            Color.FromArgb(245, 222, 179),
            Color.FromArgb(255, 222, 173),
            Color.FromArgb(255, 218, 185),
            Color.FromArgb(238, 232, 170),
            Color.FromArgb(152, 251, 152),
            Color.FromArgb(175, 238, 238),
            Color.FromArgb(176, 224, 230),
            Color.FromArgb(173, 216, 230),
            Color.FromArgb(135, 206, 250),
            Color.FromArgb(176, 196, 222),
            Color.FromArgb(216, 191, 216),
            Color.FromArgb(221, 160, 221),
            Color.FromArgb(238, 130, 238),
            Color.FromArgb(255, 182, 193),
            Color.FromArgb(255, 192, 203),
            Color.FromArgb(219, 112, 147),
            Color.FromArgb(255, 160, 122),
            Color.FromArgb(250, 128, 114),
            Color.FromArgb(233, 150, 122),
            Color.FromArgb(240, 128, 128),
            Color.FromArgb(205, 92, 92),
            Color.FromArgb(188, 143, 143),
            Color.FromArgb(210, 180, 140),
            Color.FromArgb(222, 184, 135),
            Color.FromArgb(244, 164, 96),
            Color.FromArgb(218, 165, 32),
            Color.FromArgb(189, 183, 107),
            Color.FromArgb(144, 238, 144),
            Color.FromArgb(143, 188, 143),
            Color.FromArgb(102, 205, 170),
            Color.FromArgb(127, 255, 212),
            Color.FromArgb(0, 250, 154),
            Color.FromArgb(72, 209, 204),
            Color.FromArgb(64, 224, 208),
            Color.FromArgb(0, 206, 209),
            Color.FromArgb(100, 149, 237),
            Color.FromArgb(106, 90, 205),
            Color.FromArgb(123, 104, 238),
            Color.FromArgb(147, 112, 219),
            Color.FromArgb(186, 85, 211),
            Color.FromArgb(218, 112, 214),
            Color.FromArgb(199, 21, 133),
            Color.FromArgb(219, 112, 147),
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

        private static Color GetCourseColor(string courseCode) =>
            PastelColors[HashString(courseCode) % PastelColors.Length];

        private static int GetMinutesFrom7AM(string timeStr) =>
            ParseTimeMinutes(timeStr) - 7 * 60;

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

        private static string? TryGetString(JsonElement el, string prop) =>
            el.TryGetProperty(prop, out var val) && val.ValueKind == JsonValueKind.String
                ? val.GetString()
                : null;

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRect(
            RectangleF rect,
            float radius
        )
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

        // ===========================================
        // Schedule Grid Paint
        // ===========================================

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
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                };
                g.DrawString(DayNames[i], headerFont, headerTextBrush, rect, sf);
            }

            // --- Draw time labels ---
            using var timeFont = new Font("Segoe UI", 7.5F);
            using var timeBrush = new SolidBrush(Color.FromArgb(120, 120, 120));
            for (int i = 0; i < TotalSlots; i++)
            {
                var y = HeaderHeight + i * SlotHeight;
                var rect = new RectangleF(2, y, TimeLabelWidth - 6, SlotHeight);
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Far,
                    LineAlignment = StringAlignment.Near,
                };
                g.DrawString(TimeLabels[i], timeFont, timeBrush, rect, sf);
            }

            // --- Draw grid lines ---
            using var gridPen = new Pen(Color.FromArgb(230, 230, 230), 1);

            for (int i = 0; i <= TotalSlots; i++)
            {
                var y = HeaderHeight + i * SlotHeight;
                g.DrawLine(gridPen, TimeLabelWidth, y, gridWidth, y);
            }

            g.DrawLine(gridPen, TimeLabelWidth, 0, TimeLabelWidth, HeaderHeight + gridBodyHeight);
            for (int i = 0; i <= 7; i++)
            {
                var x = TimeLabelWidth + i * dayColWidth;
                g.DrawLine(gridPen, x, 0, x, HeaderHeight + gridBodyHeight);
            }

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

                    if (
                        !course.TryGetProperty("schedule", out var schedArr)
                        || schedArr.ValueKind != JsonValueKind.Array
                    )
                        continue;

                    foreach (var sched in schedArr.EnumerateArray())
                    {
                        var timeFrom = TryGetString(sched, "timeFrom") ?? "";
                        var timeTo = TryGetString(sched, "timeTo") ?? "";
                        var roomName = TryGetString(sched, "roomName") ?? "";

                        if (
                            !sched.TryGetProperty("day", out var dayArr)
                            || dayArr.ValueKind != JsonValueKind.Array
                        )
                            continue;

                        var startMins = GetMinutesFrom7AM(timeFrom);
                        var endMins = GetMinutesFrom7AM(timeTo);
                        var durationMins = endMins - startMins;
                        if (durationMins <= 0)
                            continue;

                        var minsFrom730 = startMins - 30;
                        if (minsFrom730 < 0)
                            continue;

                        var yPos = HeaderHeight + (minsFrom730 / 30.0) * SlotHeight;
                        var blockHeight = (durationMins / 30.0) * SlotHeight - 6;
                        if (blockHeight < 10)
                            blockHeight = 10;

                        foreach (var dayEl in dayArr.EnumerateArray())
                        {
                            var dayCode = dayEl.TryGetProperty("code", out var dc)
                                ? dc.GetString()
                                : null;
                            if (
                                dayCode is null
                                || !DayCodeMap.TryGetValue(dayCode, out var dayIndex)
                            )
                                continue;

                            var xPos = TimeLabelWidth + dayIndex * dayColWidth + 2;
                            var blockWidth = dayColWidth - 4;
                            var blockRect = new RectangleF(
                                xPos,
                                (float)yPos,
                                blockWidth,
                                (float)blockHeight
                            );

                            using var blockBrush = new SolidBrush(color);
                            using var blockPath = CreateRoundedRect(blockRect, 4);
                            g.FillPath(blockBrush, blockPath);

                            using var borderColor = new SolidBrush(Color.FromArgb(40, 0, 0, 0));
                            using var borderPen2 = new Pen(borderColor, 1);
                            g.DrawPath(borderPen2, blockPath);

                            using var textSf = new StringFormat
                            {
                                Trimming = StringTrimming.EllipsisCharacter,
                                FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip,
                            };

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

                            newBlocks.Add(
                                new ScheduleBlock(
                                    blockRect,
                                    courseCode,
                                    description,
                                    section,
                                    instructor,
                                    timeFrom,
                                    timeTo,
                                    roomName
                                )
                            );
                        }
                    }
                }
            }

            _scheduleBlocks = newBlocks;
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

            if (hit == _hoveredBlock)
                return;
            _hoveredBlock = hit;

            if (hit is not null)
            {
                var tip =
                    $"{hit.CourseCode} - {hit.Section}\n{hit.Description}\n{hit.Instructor}\n{hit.TimeFrom} - {hit.TimeTo}\nRoom: {hit.RoomName}";
                _gridTooltip.SetToolTip(scheduleGridPanel, tip);
                scheduleGridPanel.Cursor = Cursors.Hand;
            }
            else
            {
                _gridTooltip.SetToolTip(scheduleGridPanel, null);
                scheduleGridPanel.Cursor = Cursors.Default;
            }
        }
    }
}
