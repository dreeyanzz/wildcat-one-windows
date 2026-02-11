using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using wildcat_one_windows.Config;

namespace wildcat_one_windows.Services;

public static class CryptoHelper
{
    public static string EncryptPayload(object data)
    {
        var json = JsonSerializer.Serialize(data);
        var key = SHA256.HashData(Encoding.UTF8.GetBytes(AppConfig.ENCRYPTION_KEY));
        var iv = Encoding.UTF8.GetBytes(AppConfig.ENCRYPTION_KEY[..16]);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(json);
        var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return Convert.ToBase64String(encrypted);
    }

    public static JsonElement DecryptPayload(string encrypted)
    {
        var key = SHA256.HashData(Encoding.UTF8.GetBytes(AppConfig.ENCRYPTION_KEY));
        var iv = Encoding.UTF8.GetBytes(AppConfig.ENCRYPTION_KEY[..16]);
        var cipherBytes = Convert.FromBase64String(encrypted);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        var decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        var json = Encoding.UTF8.GetString(decrypted);

        return JsonSerializer.Deserialize<JsonElement>(json);
    }

    public static string GenerateHmac(string nonce, string origin, string method, string salt)
    {
        var message = $"{nonce}:{origin}:{method}:{salt}:{AppConfig.CLIENT_SECRET}";
        var keyBytes = Encoding.UTF8.GetBytes(AppConfig.HMAC_SECRET);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        var hash = HMACSHA256.HashData(keyBytes, messageBytes);
        return Convert.ToHexStringLower(hash);
    }

    public static string GenerateSalt()
    {
        var bytes = RandomNumberGenerator.GetBytes(16);
        return Convert.ToBase64String(bytes);
    }
}
