using System.Text.Json;
using wildcat_one_windows.Config;

namespace wildcat_one_windows.Services;

public static class CourseOfferingsService
{
    public static async Task<ApiResponse> LoadCoursesAsync()
    {
        var session = SessionManager.Instance;
        var academicYearId = session.Get<string>("currentAcademicYearId");
        var termId = session.Get<string>("currentTermId");

        if (academicYearId is null || termId is null)
            throw new Exception("Session data missing. Please re-login.");

        return await ApiService.PostAsync(
            "/api/program/acadyear/term/coursecode",
            new { idAcademicYear = academicYearId, idTerm = termId },
            AppConfig.LOGIN_URL
        );
    }

    public static async Task<ApiResponse> SearchOfferingsAsync(string idCourse)
    {
        var session = SessionManager.Instance;
        var academicYearId = session.Get<string>("currentAcademicYearId");
        var termId = session.Get<string>("currentTermId");
        var studentInfo = session.Get<JsonElement?>("studentInfo");

        string? idBranch = null;
        if (studentInfo is JsonElement info)
        {
            if (info.TryGetProperty("idBranch", out var branchProp))
                idBranch = branchProp.ToString();
        }

        return await ApiService.PostAsync(
            "/api/courseoffering/search",
            new
            {
                idBranch,
                idProgram = (string?)null,
                idYearLevel = (string?)null,
                idAcademicYear = academicYearId,
                idTerm = termId,
                idSectionBlock = (string?)null,
                idCourse,
                courseTitle = "",
            }
        );
    }
}
