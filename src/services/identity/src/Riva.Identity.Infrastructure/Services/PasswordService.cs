using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private const int IterationsCount = 1000;
        private const int SubKeyLength = 256 / 8;
        private const int SaltSize = 128 / 8;

        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] subKey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, IterationsCount))
            {
                salt = deriveBytes.Salt;
                subKey = deriveBytes.GetBytes(SubKeyLength);
            }

            var outputBytes = new byte[1 + SaltSize + SubKeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subKey, 0, outputBytes, 1 + SaltSize, SubKeyLength);
            return Convert.ToBase64String(outputBytes);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            if (hashedPasswordBytes.Length != 1 + SaltSize + SubKeyLength || hashedPasswordBytes[0] != 0x00)
                return false;

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            var storedSubKey = new byte[SubKeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubKey, 0, SubKeyLength);

            byte[] generatedSubKey;
            using (var deriveBytes = new Rfc2898DeriveBytes(providedPassword, salt, IterationsCount))
            {
                generatedSubKey = deriveBytes.GetBytes(SubKeyLength);
            }

            return ByteArraysEqual(storedSubKey, generatedSubKey);
        }

        private static bool ByteArraysEqual(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null || a.Count != b.Count)
                return false;

            var areSame = true;

            for (var i = 0; i < a.Count; i++)
            {
                areSame &= a[i] == b[i];
            }

            return areSame;
        }
    }
}