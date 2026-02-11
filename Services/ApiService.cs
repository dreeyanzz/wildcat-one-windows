using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using wildcat_one_windows.Config;
using wildcat_one_windows.Exceptions;

namespace wildcat_one_windows.Services;

public class ApiOptions
{
    public bool IsLoginRequest { get; set; }
}

public static class ApiService
{
    private static readonly HttpClient HttpClient = new(new HttpClientHandler())
    {
        Timeout = Timeout.InfiniteTimeSpan, // We handle timeout manually with CancellationTokenSource
        DefaultRequestHeaders =
        {
            {
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
            },
        },
    };

    public static async Task<ApiResponse> CallAsync(
        HttpMethod method,
        string endpoint,
        object? data = null,
        string? baseUrl = null,
        ApiOptions? options = null
    )
    {
        baseUrl ??= AppConfig.BASE_URL;
        options ??= new ApiOptions();

        var fullUrl = $"{baseUrl}{endpoint}";
        var encrypted = data is not null ? CryptoHelper.EncryptPayload(data) : null;
        var body = encrypted is not null ? JsonSerializer.Serialize(new { encrypted }) : null;

        var nonce = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}-{Random.Shared.Next(10000)}";
        var salt = CryptoHelper.GenerateSalt();
        var signature = CryptoHelper.GenerateHmac(nonce, "studentportal", method.Method, salt);

        using var request = new HttpRequestMessage(method, fullUrl);
        request.Headers.Add("Accept", "application/json, text/plain, */*");
        request.Headers.Add("X-Origin", "studentportal");
        request.Headers.Add("X-HMAC-Nonce", nonce);
        request.Headers.Add("X-HMAC-Salt", salt);
        request.Headers.Add("X-HMAC-Signature", signature);

        var token = SessionManager.Instance.Token;
        if (token is not null)
            request.Headers.Add("Authorization", $"Bearer {token}");

        if (body is not null)
        {
            request.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(body));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        using var cts = new CancellationTokenSource(AppConfig.API_TIMEOUT);

        try
        {
            var response = await HttpClient.SendAsync(request, cts.Token);
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
            var responseText = await response.Content.ReadAsStringAsync(cts.Token);

            if (string.IsNullOrWhiteSpace(responseText))
            {
                if (!response.IsSuccessStatusCode)
                    return new ApiResponse(
                        (int)response.StatusCode,
                        CreateMessageElement(response.ReasonPhrase ?? "Error"),
                        false
                    );

                throw new ApiException("Empty response from server", (int)response.StatusCode);
            }

            JsonElement responseData;
            try
            {
                responseData = contentType.Contains(
                    "text/plain",
                    StringComparison.OrdinalIgnoreCase
                )
                    ? CryptoHelper.DecryptPayload(responseText)
                    : JsonSerializer.Deserialize<JsonElement>(responseText);
            }
            catch (Exception parseError)
            {
                if (!response.IsSuccessStatusCode)
                    return new ApiResponse(
                        (int)response.StatusCode,
                        CreateMessageElement(responseText),
                        false
                    );

                throw new ApiException(
                    "Failed to parse server response",
                    (int)response.StatusCode,
                    parseError
                );
            }

            if ((int)response.StatusCode == 401 && !options.IsLoginRequest)
            {
                HandleTokenExpiration();
            }

            return new ApiResponse(
                (int)response.StatusCode,
                responseData,
                response.IsSuccessStatusCode
            );
        }
        catch (ApiException)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            throw new ApiException("Request timed out. Please try again.", 0);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException("Network error. Please check your connection.", 0, ex);
        }
        catch (Exception ex) when (ex is not ApiException)
        {
            throw new ApiException(ex.Message, 0, ex);
        }
    }

    public static Task<ApiResponse> GetAsync(string endpoint, string? baseUrl = null) =>
        CallAsync(HttpMethod.Get, endpoint, null, baseUrl);

    public static Task<ApiResponse> PostAsync(
        string endpoint,
        object data,
        string? baseUrl = null,
        ApiOptions? options = null
    ) => CallAsync(HttpMethod.Post, endpoint, data, baseUrl, options);

    private static void HandleTokenExpiration()
    {
        SessionManager.Instance.Reset();
    }

    private static JsonElement CreateMessageElement(string message)
    {
        var json = JsonSerializer.Serialize(new { message });
        return JsonSerializer.Deserialize<JsonElement>(json);
    }
}

public record ApiResponse(int Status, JsonElement Data, bool Success);
