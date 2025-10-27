using System.Security.Cryptography;
using System.Text;
namespace Contribute.Service;
public class SecretService
{
    public string ComputeHash(string input)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        StringBuilder builder = new();
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }

    public bool VerifyHash(string input, string? hash)
    {
        if (hash == null || input == null)
        {
            return false;
        }
        string inputHash = ComputeHash(input);
        return inputHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }
}