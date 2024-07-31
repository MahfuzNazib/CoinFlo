using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace CoinFlo.API.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Derive a subkey (use HMACSHA256 with 100,000 iterations)
            var hashedPassword = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32 // Length of the derived subkey
            );

            // Combine salt and hashed password for storage
            var combinedHash = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hashedPassword);

            return combinedHash;
        }

        public static bool VerifyPassword(string password, string combinedHash)
        {
            // Split combined hash to extract salt
            var parts = combinedHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(parts[0]);

            // Derive a new subkey using the same parameters
            var newHashedPassword = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32
            );

            // Compare the hashed passwords
            return SecureEquals(newHashedPassword, Convert.FromBase64String(parts[1]));
        }

        private static bool SecureEquals(byte[] a, byte[] b)
        {
            var length = a.Length;
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}
