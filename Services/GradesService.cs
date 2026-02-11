using System.Text.Json;

namespace wildcat_one_windows.Services;

public static class GradesService
{
    /// <summary>
    /// Fetches the studentEnrollments array from the grades API.
    /// Returns an empty list on failure or missing data.
    /// </summary>
    public static async Task<List<JsonElement>> FetchEnrollmentsAsync()
    {
        var session = SessionManager.Instance;
        var studentId = session.UserData?.GetProperty("studentId").ToString();
        var studentInfo = session.Get<JsonElement?>("studentInfo");

        if (studentId is null || studentInfo is null)
            return [];

        string? departmentId = null;
        if (studentInfo.Value.TryGetProperty("idDepartment", out var deptProp))
            departmentId = deptProp.ToString();
        if (departmentId is null && studentInfo.Value.TryGetProperty("departmentId", out var deptProp2))
            departmentId = deptProp2.ToString();

        if (departmentId is null)
            return [];

        var result = await ApiService.GetAsync(
            $"/api/studentgradefile/student/{studentId}/department/{departmentId}");

        if (!result.Success || !result.Data.TryGetProperty("items", out var items))
            return [];

        if (!items.TryGetProperty("studentEnrollments", out var enrollments)
            || enrollments.ValueKind != JsonValueKind.Array)
            return [];

        return enrollments.EnumerateArray().ToList();
    }
}
