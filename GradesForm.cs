using System.Text.Json;
using wildcat_one_windows.Services;

namespace wildcat_one_windows
{
    public partial class GradesForm : Form
    {
        private List<JsonElement> _gradeEnrollments = [];

        public GradesForm()
        {
            InitializeComponent();
            gradesSemesterComboBox.SelectedIndexChanged += GradesSemesterComboBox_SelectedIndexChanged;
            gradesDataGridView.CellFormatting += GradesDataGridView_CellFormatting;
            gradesTabControl.SelectedIndexChanged += GradesTabControl_SelectedIndexChanged;
            Load += async (_, _) => await LoadGradesPageAsync();
        }

        private async Task LoadGradesPageAsync()
        {
            gradesSemesterComboBox.Items.Clear();
            gradesSemesterComboBox.Items.Add("Loading semesters...");
            gradesSemesterComboBox.SelectedIndex = 0;
            gradesSemesterComboBox.Enabled = false;
            gradesEmptyLabel.Visible = false;

            try
            {
                _gradeEnrollments = await GradesService.FetchEnrollmentsAsync();

                gradesSemesterComboBox.Items.Clear();

                if (_gradeEnrollments.Count == 0)
                {
                    gradesSemesterComboBox.Items.Add("No semesters available");
                    gradesSemesterComboBox.SelectedIndex = 0;
                    gradesDataGridView.Rows.Clear();
                    gradesEmptyLabel.Text = "No grades available";
                    gradesEmptyLabel.Visible = true;
                    gradesTableContainer.Visible = false;
                    return;
                }

                foreach (var enrollment in _gradeEnrollments)
                {
                    var academicYear = TryGetString(enrollment, "academicYear") ?? "N/A";
                    var term = TryGetString(enrollment, "term") ?? "N/A";
                    var yearLevel = TryGetString(enrollment, "yearLevel") ?? "N/A";
                    gradesSemesterComboBox.Items.Add($"{academicYear}: {term} ({yearLevel})");
                }

                gradesSemesterComboBox.Enabled = true;
                gradesTableContainer.Visible = true;
                gradesSemesterComboBox.SelectedIndex = 0;
            }
            catch
            {
                gradesSemesterComboBox.Items.Clear();
                gradesSemesterComboBox.Items.Add("Failed to load semesters");
                gradesSemesterComboBox.SelectedIndex = 0;
                gradesDataGridView.Rows.Clear();
                gradesEmptyLabel.Text = "Failed to load grades. Please try again.";
                gradesEmptyLabel.Visible = true;
                gradesTableContainer.Visible = false;
            }
        }

        private void GradesSemesterComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!gradesSemesterComboBox.Enabled || _gradeEnrollments.Count == 0)
                return;
            var idx = gradesSemesterComboBox.SelectedIndex;
            if (idx < 0 || idx >= _gradeEnrollments.Count)
                return;

            var enrollment = _gradeEnrollments[idx];

            var academicYear = TryGetString(enrollment, "academicYear") ?? "N/A";
            var term = TryGetString(enrollment, "term") ?? "N/A";
            var yearLevel = TryGetString(enrollment, "yearLevel") ?? "N/A";

            var status = "N/A";
            if (enrollment.TryGetProperty("idEnrollmentStatus", out var statusProp))
            {
                if (statusProp.ToString() == "2")
                    status = "Enrolled";
            }

            var gwaText = "";
            if (enrollment.TryGetProperty("gwa", out var gwaProp))
            {
                if (double.TryParse(gwaProp.ToString(), out var gwa) && gwa > 0)
                    gwaText = $"  |  GWA: {gwa:F2}";
            }

            gradesSemesterInfoLabel.Text =
                $"School Year: {academicYear}  |  Semester: {term}  |  Year Level: {yearLevel}  |  Status: {status}{gwaText}";

            PopulateGradesTable(enrollment);
            UpdateGradeSummaryTab(enrollment);
        }

        private void GradesTabControl_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Refresh summary when switching to the Summary tab
            if (gradesTabControl.SelectedIndex == 1 && _gradeEnrollments.Count > 0)
            {
                var idx = gradesSemesterComboBox.SelectedIndex;
                if (idx >= 0 && idx < _gradeEnrollments.Count)
                    UpdateGradeSummaryTab(_gradeEnrollments[idx]);
            }
        }

        private void UpdateGradeSummaryTab(JsonElement enrollment)
        {
            var academicYear = TryGetString(enrollment, "academicYear") ?? "N/A";
            var term = TryGetString(enrollment, "term") ?? "N/A";
            var yearLevel = TryGetString(enrollment, "yearLevel") ?? "N/A";

            var status = "N/A";
            if (enrollment.TryGetProperty("idEnrollmentStatus", out var statusProp))
            {
                if (statusProp.ToString() == "2")
                    status = "Enrolled";
            }

            var gwaDisplay = "N/A";
            if (enrollment.TryGetProperty("gwa", out var gwaProp))
            {
                if (double.TryParse(gwaProp.ToString(), out var gwa) && gwa > 0)
                    gwaDisplay = gwa.ToString("F2");
            }

            summaryGwaLabel.Text = $"GWA: {gwaDisplay}";
            summarySemesterLabel.Text = $"Semester: {academicYear} — {term}";
            summaryStatusLabel.Text = $"Status: {status}";
            summaryYearLevelLabel.Text = $"Year Level: {yearLevel}";
        }

        private void PopulateGradesTable(JsonElement enrollment)
        {
            gradesDataGridView.Rows.Clear();

            JsonElement courses = default;
            bool hasCourses = false;

            if (
                enrollment.TryGetProperty("enrolledCourseGradeDetails", out var ecgd)
                && ecgd.ValueKind == JsonValueKind.Array
            )
            {
                courses = ecgd;
                hasCourses = true;
            }
            else if (
                enrollment.TryGetProperty("studentGradeHistoryData", out var sghd)
                && sghd.ValueKind == JsonValueKind.Array
            )
            {
                courses = sghd;
                hasCourses = true;
            }

            if (!hasCourses || courses.GetArrayLength() == 0)
            {
                gradesEmptyLabel.Text = "No grades available for this semester";
                gradesEmptyLabel.Visible = true;
                gradesTableContainer.Visible = false;
                return;
            }

            gradesEmptyLabel.Visible = false;
            gradesTableContainer.Visible = true;

            foreach (var course in courses.EnumerateArray())
            {
                var courseCode = TryGetString(course, "courseCode") ?? "N/A";
                var courseTitle =
                    TryGetString(course, "courseTitle")
                    ?? TryGetString(course, "description")
                    ?? "N/A";
                var professor = TryGetString(course, "professor") ?? "No instructor assigned";
                var units = "-";
                if (course.TryGetProperty("units", out var unitsProp))
                {
                    if (double.TryParse(unitsProp.ToString(), out var unitsVal))
                        units =
                            unitsVal == Math.Floor(unitsVal)
                                ? ((int)unitsVal).ToString()
                                : unitsVal.ToString("F1");
                    else
                        units = unitsProp.ToString();
                }

                var midterm = "-";
                var final_ = "-";
                var gradeStatus = "-";

                bool hasGradeDetails =
                    course.TryGetProperty("gradeDetails", out var gradeDetails)
                    && gradeDetails.ValueKind == JsonValueKind.Array
                    && gradeDetails.GetArrayLength() > 0;

                string? gradeDetailFinalGrade = null;
                string? gradeDetailFinalStatus = null;
                if (
                    course.TryGetProperty("gradeDetailFinal", out var gdf)
                    && gdf.ValueKind == JsonValueKind.Object
                )
                {
                    gradeDetailFinalGrade = FormatGrade(TryGetString(gdf, "grade"));
                    gradeDetailFinalStatus = TryGetString(gdf, "gradeStatus");
                }

                if (hasGradeDetails)
                {
                    foreach (var gd in gradeDetails.EnumerateArray())
                    {
                        var periodName = TryGetString(gd, "periodName") ?? "";
                        var gradeVal = FormatGrade(TryGetString(gd, "grade"));
                        var hasGrade = !string.IsNullOrWhiteSpace(gradeVal);

                        if (periodName == "Midterm")
                            midterm = hasGrade ? gradeVal! : "-";
                        else if (periodName == "Final")
                            final_ = hasGrade ? gradeVal! : gradeDetailFinalGrade ?? "-";
                    }
                    if (final_ == "-")
                        final_ = gradeDetailFinalGrade ?? "-";
                }
                else
                {
                    final_ = gradeDetailFinalGrade ?? "-";
                }

                gradeStatus = gradeDetailFinalStatus ?? TryGetString(course, "remarks") ?? "-";

                var displayTitle = $"{courseTitle}\n{professor}";
                gradesDataGridView.Rows.Add(courseCode, displayTitle, units, midterm, final_, gradeStatus);
            }
        }

        private void GradesDataGridView_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e
        )
        {
            if (e.ColumnIndex != 3 && e.ColumnIndex != 4)
                return;
            if (e.Value is not string gradeStr || string.IsNullOrEmpty(gradeStr) || gradeStr == "-")
                return;

            if (!double.TryParse(gradeStr, out var grade))
                return;

            Color gradeColor;
            if (grade >= 4.5)
                gradeColor = Color.FromArgb(39, 174, 96);
            else if (grade >= 4.0)
                gradeColor = Color.FromArgb(46, 204, 113);
            else if (grade >= 3.0)
                gradeColor = Color.FromArgb(243, 156, 18);
            else if (grade >= 2.0)
                gradeColor = Color.FromArgb(230, 126, 34);
            else
                gradeColor = Color.FromArgb(231, 76, 60);

            e.CellStyle!.ForeColor = gradeColor;
            e.CellStyle.SelectionForeColor = gradeColor;
            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private static string? FormatGrade(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return raw;
            if (double.TryParse(raw, out var val))
                return val.ToString("0.0");
            return raw;
        }

        private static string? TryGetString(JsonElement el, string prop) =>
            el.TryGetProperty(prop, out var val) && val.ValueKind == JsonValueKind.String
                ? val.GetString()
                : null;
    }
}
