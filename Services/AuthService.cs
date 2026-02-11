using System.Text.Json;
using wildcat_one_windows.Config;
using wildcat_one_windows.Exceptions;
using wildcat_one_windows.Utils;

namespace wildcat_one_windows.Services;

public static class AuthService
{
    public static async Task<AuthResult> LoginAsync(string userId, string password)
    {
        Validator.ValidateRequired(userId, "Student ID");
        Validator.ValidateRequired(password, "Password");
        Validator.ValidateStudentId(userId);

        var result = await ApiService.PostAsync(
            "/api/User/student/login",
            new { userId, password, clientId = AppConfig.CLIENT_ID },
            AppConfig.LOGIN_URL,
            new ApiOptions { IsLoginRequest = true });

        if (result.Status == 401)
            throw new AuthenticationException("Invalid credentials. Please check your Student ID and password.");

        if (!result.Success)
        {
            var serverMsg = "Server error";
            if (result.Data.ValueKind == JsonValueKind.Object && result.Data.TryGetProperty("message", out var msgProp))
                serverMsg = msgProp.GetString() ?? serverMsg;
            throw new ApiException($"Login failed (HTTP {result.Status}): {serverMsg}", result.Status);
        }

        if (!result.Data.TryGetProperty("token", out var tokenElement))
            throw new AuthenticationException("Login succeeded but no authentication token was received.");

        var token = tokenElement.GetString()
            ?? throw new AuthenticationException("Login succeeded but no authentication token was received.");

        var userInfo = result.Data.GetProperty("userInfo");

        SessionManager.Instance.Token = token;
        SessionManager.Instance.UserData = userInfo;

        await InitializeAcademicContext(userInfo);

        return new AuthResult(true, userInfo);
    }

    public static async Task<ForgotPasswordResult> ForgotPasswordAsync(string studentId, DateTime birthdate)
    {
        Validator.ValidateRequired(studentId, "Student ID");
        Validator.ValidateStudentId(studentId);

        var utcBirthdate = new DateTime(birthdate.Year, birthdate.Month, birthdate.Day, 0, 0, 0, DateTimeKind.Utc);
        var formattedBirthdate = utcBirthdate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

        var result = await ApiService.PostAsync(
            "/api/user/student/forgotpassword",
            new { studentID = studentId, studentBirthDate = formattedBirthdate },
            AppConfig.LOGIN_URL,
            new ApiOptions { IsLoginRequest = true });

        if (result.Status == 404)
        {
            var msg = "Record does not exist. Please check your Student ID and birthdate.";
            if (result.Data.ValueKind == JsonValueKind.Object && result.Data.TryGetProperty("message", out var msgProp))
                msg = msgProp.GetString() ?? msg;
            throw new AuthenticationException(msg);
        }

        if (!result.Success)
            throw new AuthenticationException("Password reset failed. Please try again.");

        var message = "Password reset is successful. Please check your email.";
        if (result.Data.ValueKind == JsonValueKind.Object && result.Data.TryGetProperty("message", out var responseMsgProp))
            message = responseMsgProp.GetString() ?? message;

        return new ForgotPasswordResult(true, message);
    }

    public static void Logout()
    {
        SessionManager.Instance.Reset();
    }

    public static bool IsAuthenticated
        => SessionManager.Instance.Token is not null && SessionManager.Instance.UserData is not null;

    private static async Task InitializeAcademicContext(JsonElement userData)
    {
        var userId = userData.GetProperty("userId").ToString();
        var studentId = userData.GetProperty("studentId").ToString();

        // Fetch student info
        var infoResult = await ApiService.GetAsync(
            $"/api/user/student/{userId}/info",
            AppConfig.LOGIN_URL);

        if (infoResult.Status != 200 || !infoResult.Data.TryGetProperty("items", out var studentInfo))
            throw new ApiException("Failed to fetch student information", infoResult.Status);

        SessionManager.Instance.Set("studentInfo", studentInfo);

        // Fetch academic years
        var yearsResult = await ApiService.GetAsync($"/api/student/{studentId}/academicyears");

        if (yearsResult.Status != 200 || !yearsResult.Data.TryGetProperty("items", out var yearsElement))
            throw new ApiException("Failed to fetch academic years", yearsResult.Status);

        var years = yearsElement.EnumerateArray().ToList();
        SessionManager.Instance.Set("academicYears", yearsElement);

        string? currentYearId = null;

        if (studentInfo.TryGetProperty("academicYear", out var academicYearProp))
        {
            var academicYearName = academicYearProp.GetString();
            foreach (var year in years)
            {
                if (year.TryGetProperty("name", out var nameProp) && nameProp.GetString() == academicYearName)
                {
                    currentYearId = year.GetProperty("id").ToString();
                    SessionManager.Instance.Set("currentAcademicYearName", academicYearName);
                    break;
                }
            }
        }

        if (currentYearId is null && years.Count > 0)
        {
            var latestYear = years[^1];
            currentYearId = latestYear.GetProperty("id").ToString();
            SessionManager.Instance.Set("currentAcademicYearName",
                latestYear.GetProperty("name").GetString());
        }

        if (currentYearId is null)
            throw new ApiException("No academic years available", 0);

        SessionManager.Instance.Set("currentAcademicYearId", currentYearId);

        // Fetch terms
        await InitializeTerms(studentId, currentYearId, studentInfo);
    }

    private static async Task InitializeTerms(string studentId, string yearId, JsonElement studentInfo)
    {
        var termsResult = await ApiService.GetAsync($"/api/student/{studentId}/{yearId}/terms");

        if (termsResult.Status != 200 || !termsResult.Data.TryGetProperty("items", out var termsElement))
            throw new ApiException("Failed to fetch terms", termsResult.Status);

        var terms = termsElement.EnumerateArray().ToList();
        SessionManager.Instance.Set("availableTerms", termsElement);

        string? currentTermId = null;

        if (studentInfo.TryGetProperty("term", out var termProp))
        {
            var termName = termProp.GetString();
            foreach (var term in terms)
            {
                if (term.TryGetProperty("name", out var nameProp) && nameProp.GetString() == termName)
                {
                    currentTermId = term.GetProperty("id").ToString();
                    SessionManager.Instance.Set("currentTermName", termName);
                    break;
                }
            }
        }

        if (currentTermId is null && terms.Count > 0)
        {
            var latestTerm = terms[^1];
            currentTermId = latestTerm.GetProperty("id").ToString();
            SessionManager.Instance.Set("currentTermName",
                latestTerm.GetProperty("name").GetString());
        }

        if (currentTermId is null)
            throw new ApiException("No terms available", 0);

        SessionManager.Instance.Set("currentTermId", currentTermId);
    }
}

public record AuthResult(bool Success, JsonElement UserData);
public record ForgotPasswordResult(bool Success, string Message);
