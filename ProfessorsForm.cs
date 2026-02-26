using System.Text.Json;
using wildcat_one_windows.Services;

namespace wildcat_one_windows
{
    public partial class ProfessorsForm : Form
    {
        private List<JsonElement> _profEnrollments = [];

        public ProfessorsForm()
        {
            InitializeComponent();
            profSemesterComboBox.SelectedIndexChanged += ProfSemesterComboBox_SelectedIndexChanged;
            Load += async (_, _) => await LoadProfessorsPageAsync();
        }

        private async Task LoadProfessorsPageAsync()
        {
            profSemesterComboBox.Items.Clear();
            profSemesterComboBox.Items.Add("Loading semesters...");
            profSemesterComboBox.SelectedIndex = 0;
            profSemesterComboBox.Enabled = false;
            profEmptyLabel.Visible = false;

            try
            {
                _profEnrollments = await GradesService.FetchEnrollmentsAsync();

                profSemesterComboBox.Items.Clear();

                if (_profEnrollments.Count == 0)
                {
                    profSemesterComboBox.Items.Add("No semesters available");
                    profSemesterComboBox.SelectedIndex = 0;
                    profDataGridView.Rows.Clear();
                    profEmptyLabel.Text = "No professor data available";
                    profEmptyLabel.Visible = true;
                    profTableContainer.Visible = false;
                    return;
                }

                foreach (var enrollment in _profEnrollments)
                {
                    var academicYear = TryGetString(enrollment, "academicYear") ?? "N/A";
                    var term = TryGetString(enrollment, "term") ?? "N/A";
                    var yearLevel = TryGetString(enrollment, "yearLevel") ?? "N/A";
                    profSemesterComboBox.Items.Add($"{academicYear}: {term} ({yearLevel})");
                }

                profSemesterComboBox.Enabled = true;
                profTableContainer.Visible = true;
                profSemesterComboBox.SelectedIndex = 0;
            }
            catch
            {
                profSemesterComboBox.Items.Clear();
                profSemesterComboBox.Items.Add("Failed to load semesters");
                profSemesterComboBox.SelectedIndex = 0;
                profDataGridView.Rows.Clear();
                profEmptyLabel.Text = "Failed to load professors. Please try again.";
                profEmptyLabel.Visible = true;
                profTableContainer.Visible = false;
            }
        }

        private void ProfSemesterComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!profSemesterComboBox.Enabled || _profEnrollments.Count == 0)
                return;
            var idx = profSemesterComboBox.SelectedIndex;
            if (idx < 0 || idx >= _profEnrollments.Count)
                return;

            var enrollment = _profEnrollments[idx];

            var academicYear = TryGetString(enrollment, "academicYear") ?? "N/A";
            var term = TryGetString(enrollment, "term") ?? "N/A";
            var yearLevel = TryGetString(enrollment, "yearLevel") ?? "N/A";

            profSemesterInfoLabel.Text =
                $"School Year: {academicYear}  |  Semester: {term}  |  Year Level: {yearLevel}";

            PopulateProfessorsTable(enrollment);
        }

        private void PopulateProfessorsTable(JsonElement enrollment)
        {
            profDataGridView.Rows.Clear();

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
                profEmptyLabel.Text = "No professor data available for this semester";
                profEmptyLabel.Visible = true;
                profTableContainer.Visible = false;
                return;
            }

            profEmptyLabel.Visible = false;
            profTableContainer.Visible = true;

            foreach (var course in courses.EnumerateArray())
            {
                var courseCode = TryGetString(course, "courseCode") ?? "N/A";
                var courseTitle =
                    TryGetString(course, "courseTitle")
                    ?? TryGetString(course, "description")
                    ?? "N/A";
                var professor = TryGetString(course, "professor") ?? "No instructor assigned";
                profDataGridView.Rows.Add(courseCode, courseTitle, professor);
            }
        }

        // ===========================================
        // ContextMenuStrip Handlers
        // ===========================================

        private void CtxCopyProfName_Click(object? sender, EventArgs e)
        {
            if (profDataGridView.CurrentRow is DataGridViewRow row)
            {
                var prof = row.Cells["ProfProfessor"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(prof))
                    Clipboard.SetText(prof);
            }
        }

        private void CtxCopyCourseCode_Click(object? sender, EventArgs e)
        {
            if (profDataGridView.CurrentRow is DataGridViewRow row)
            {
                var code = row.Cells["ProfCourseCode"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(code))
                    Clipboard.SetText(code);
            }
        }

        private static string? TryGetString(JsonElement el, string prop) =>
            el.TryGetProperty(prop, out var val) && val.ValueKind == JsonValueKind.String
                ? val.GetString()
                : null;
    }
}
