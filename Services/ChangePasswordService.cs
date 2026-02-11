using wildcat_one_windows.Config;

namespace wildcat_one_windows.Services;

public static class ChangePasswordService
{
    public static Task<ApiResponse> RequestOtpAsync(string oldPassword, string newPassword)
    {
        return ApiService.PostAsync(
            "/api/usermaster/student/otp",
            new { oldPassword, newPassword },
            AppConfig.LOGIN_URL
        );
    }

    public static Task<ApiResponse> SubmitPasswordChangeAsync(
        string otp,
        string oldPassword,
        string newPassword
    )
    {
        return ApiService.PostAsync(
            "/api/usermaster/student/changepassword",
            new
            {
                oneTimePassword = otp,
                oldPassword,
                newPassword,
            },
            AppConfig.LOGIN_URL
        );
    }
}
