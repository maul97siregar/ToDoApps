using System;
using System.Security.Cryptography;

public class KeyGenerator
{
    public static string GenerateSecureKey(int sizeInBytes)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[sizeInBytes];
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
