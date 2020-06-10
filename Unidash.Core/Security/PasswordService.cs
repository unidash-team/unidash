using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace Unidash.Core.Security
{
    public class PasswordService
    {
        public string Hash(string rawPassword, string base64Salt) =>
            Hash(rawPassword, Convert.FromBase64String(base64Salt));

        public string Hash(string rawPassword, byte[] salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                rawPassword,
                salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return hashed;
        }

        public bool Validate(string rawPassword, string base64Salt, string hashedPassword) =>
            Validate(rawPassword, Convert.FromBase64String(base64Salt), hashedPassword);

        public bool Validate(string rawPassword, byte[] salt, string hashedPassword)
        {
            return Hash(rawPassword, salt) == hashedPassword;
        }

        public string GenerateRandomSaltAsBase64() => Convert.ToBase64String(GenerateRandomSalt());

        public byte[] GenerateRandomSalt()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }
    }
}