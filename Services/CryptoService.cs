using System.Security.Cryptography;
using System.Text;

namespace Lab1_Vardzar_2_4.Services;

public interface ICryptoService
{
    Task<string> EncryptAsync(string plainText);
    Task<string> DecryptAsync(string cipherText);
}

public class CryptoService : ICryptoService
{
    private const string KeyName = "student_db_key";
    private byte[]? _key;

    private async Task<byte[]> GetKeyAsync()
    {
        if (_key is not null) return _key;

        string? base64 = null;
        try { base64 = await SecureStorage.Default.GetAsync(KeyName); } catch { }

        if (string.IsNullOrEmpty(base64))
            base64 = Preferences.Get(KeyName, "");

        if (string.IsNullOrEmpty(base64))
        {
            base64 = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            try { await SecureStorage.Default.SetAsync(KeyName, base64); }
            catch { Preferences.Set(KeyName, base64); }
        }

        _key = Convert.FromBase64String(base64);
        return _key;
    }

    public async Task<string> EncryptAsync(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return string.Empty;

        var key = await GetKeyAsync();
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();  

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        var result = new byte[aes.IV.Length + cipherBytes.Length];
        aes.IV.CopyTo(result, 0);
        cipherBytes.CopyTo(result, aes.IV.Length);
        return Convert.ToBase64String(result);
    }

    public async Task<string> DecryptAsync(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return string.Empty;

        try
        {
            var key = await GetKeyAsync();
            var all = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = all[..16];   

            using var decryptor = aes.CreateDecryptor();
            var data = all[16..];
            var plain = decryptor.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(plain);
        }
        catch (Exception ex) when (ex is FormatException or CryptographicException)
        {
            return string.Empty;   
        }
    }
}