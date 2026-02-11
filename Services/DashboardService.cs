using System.Text.Json;

namespace wildcat_one_windows.Services;

public static class DashboardService
{
    public static async Task<JsonElement?> FetchScheduleAsync()
    {
        var session = SessionManager.Instance;
        var studentId = session.UserData?.GetProperty("studentId").ToString();
        var yearId = session.Get<string>("currentAcademicYearId");
        var termId = session.Get<string>("currentTermId");

        if (studentId is null || yearId is null || termId is null)
            return null;

        try
        {
            var result = await ApiService.GetAsync(
                $"/api/student/{studentId}/{yearId}/{termId}/schedule"
            );
            if (result.Success && result.Data.TryGetProperty("items", out var items))
                return items;
        }
        catch { }

        return null;
    }

    public static async Task<JsonElement?> FetchGradesAsync()
    {
        var session = SessionManager.Instance;
        var studentId = session.UserData?.GetProperty("studentId").ToString();
        var studentInfo = session.Get<JsonElement?>("studentInfo");

        if (studentId is null || studentInfo is null)
            return null;

        // Use idDepartment (not departmentId) per API structure
        string? departmentId = null;
        if (studentInfo.Value.TryGetProperty("idDepartment", out var deptProp))
            departmentId = deptProp.ToString();
        // Fallback to departmentId
        if (
            departmentId is null
            && studentInfo.Value.TryGetProperty("departmentId", out var deptProp2)
        )
            departmentId = deptProp2.ToString();

        if (departmentId is null)
            return null;

        try
        {
            var result = await ApiService.GetAsync(
                $"/api/studentgradefile/student/{studentId}/department/{departmentId}"
            );
            // Return the full "items" object (which contains studentEnrollments)
            if (result.Success && result.Data.TryGetProperty("items", out var items))
                return items;
        }
        catch { }

        return null;
    }
}
