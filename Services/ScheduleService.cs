using System.Text.Json;

namespace wildcat_one_windows.Services;

public record SemesterOption(
    string YearId,
    string YearName,
    string TermId,
    string TermName,
    string DisplayText
);

public static class ScheduleService
{
    public static async Task<List<JsonElement>> FetchTermsAsync(string studentId, string yearId)
    {
        var result = await ApiService.GetAsync($"/api/student/{studentId}/{yearId}/terms");
        if (
            result.Success
            && result.Data.TryGetProperty("items", out var items)
            && items.ValueKind == JsonValueKind.Array
        )
            return items.EnumerateArray().ToList();
        return [];
    }

    public static async Task<JsonElement?> FetchScheduleAsync(
        string studentId,
        string yearId,
        string termId
    )
    {
        var result = await ApiService.GetAsync(
            $"/api/student/{studentId}/{yearId}/{termId}/schedule"
        );
        if (result.Success && result.Data.TryGetProperty("items", out var items))
            return items;
        return null;
    }

    public static async Task<List<SemesterOption>> LoadAllSemesterOptionsAsync()
    {
        var session = SessionManager.Instance;
        var studentId = session.UserData?.GetProperty("studentId").ToString();
        if (studentId is null)
            return [];

        var academicYearsElement = session.Get<JsonElement?>("academicYears");
        if (
            academicYearsElement is not JsonElement yearsEl
            || yearsEl.ValueKind != JsonValueKind.Array
        )
            return [];

        var years = yearsEl.EnumerateArray().ToList();
        var options = new List<SemesterOption>();

        // Build all year+term combos
        foreach (var year in years)
        {
            var yearId = year.GetProperty("id").ToString();
            var yearName = year.TryGetProperty("name", out var yn) ? yn.GetString() ?? "" : "";

            var terms = await FetchTermsAsync(studentId, yearId);
            foreach (var term in terms)
            {
                var termId = term.GetProperty("id").ToString();
                var termName = term.TryGetProperty("name", out var tn) ? tn.GetString() ?? "" : "";
                var displayText = $"{yearName} - {termName}";
                options.Add(new SemesterOption(yearId, yearName, termId, termName, displayText));
            }
        }

        // Reverse so most recent is first
        options.Reverse();

        // Enrich with year levels from grades API
        await EnrichWithYearLevels(options, studentId);

        return options;
    }

    private static async Task EnrichWithYearLevels(List<SemesterOption> options, string studentId)
    {
        var session = SessionManager.Instance;
        var studentInfo = session.Get<JsonElement?>("studentInfo");
        if (studentInfo is not JsonElement info)
            return;

        string? departmentId = null;
        if (info.TryGetProperty("idDepartment", out var deptProp))
            departmentId = deptProp.ToString();
        if (departmentId is null && info.TryGetProperty("departmentId", out var deptProp2))
            departmentId = deptProp2.ToString();
        if (departmentId is null)
            return;

        try
        {
            var result = await ApiService.GetAsync(
                $"/api/studentgradefile/student/{studentId}/department/{departmentId}"
            );
            if (!result.Success || !result.Data.TryGetProperty("items", out var items))
                return;

            if (
                !items.TryGetProperty("studentEnrollments", out var enrollments)
                || enrollments.ValueKind != JsonValueKind.Array
            )
                return;

            var enrollmentList = enrollments.EnumerateArray().ToList();

            for (int i = 0; i < options.Count; i++)
            {
                var opt = options[i];
                foreach (var enrollment in enrollmentList)
                {
                    var enrollAcadYear = enrollment.TryGetProperty("academicYear", out var ay)
                        ? ay.GetString()
                        : null;
                    var enrollTerm = enrollment.TryGetProperty("term", out var t)
                        ? t.GetString()
                        : null;

                    if (enrollAcadYear == opt.YearName && enrollTerm == opt.TermName)
                    {
                        var yearLevel = enrollment.TryGetProperty("yearLevel", out var yl)
                            ? yl.GetString()
                            : null;
                        if (yearLevel is not null)
                        {
                            options[i] = opt with
                            {
                                DisplayText = $"{opt.DisplayText} ({yearLevel})",
                            };
                        }
                        break;
                    }
                }
            }
        }
        catch { }
    }
}
