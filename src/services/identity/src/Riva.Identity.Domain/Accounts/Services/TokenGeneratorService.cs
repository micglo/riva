using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Enumerations;

namespace Riva.Identity.Domain.Accounts.Services
{
    public class TokenGeneratorService
    {
        private static readonly DateTimeOffset UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeSpan Timestamp = TimeSpan.FromMinutes(3);
        private static readonly Encoding Encoding = new UTF8Encoding(false, true);

        public Token Generate(Guid accountId, Guid securityStamp, TokenTypeEnumeration tokenType)
        {
            var issued = DateTimeOffset.UtcNow;
            var expires = issued.AddDays(1);
            var value = GenerateTokenValue(accountId, securityStamp, tokenType);

            return Token.Builder()
                .SetIssued(issued)
                .SetExpires(expires)
                .SetType(tokenType)
                .SetValue(value)
                .Build();
        }

        private static string GenerateTokenValue(Guid accountId, Guid securityStamp, TokenTypeEnumeration tokenType)
        {
            var securityToken = Encoding.Unicode.GetBytes(securityStamp.ToString());
            var modifier = GetAccountModifier(accountId, tokenType);
            return GenerateCode(securityToken, modifier).ToString("D6", CultureInfo.InvariantCulture);
        }

        private static string GetAccountModifier(Guid accountId, TokenTypeEnumeration tokenType)
        {
            var modifier = Equals(tokenType, TokenTypeEnumeration.AccountConfirmation) ? "Email:" : "Totp:";
            return modifier + tokenType.DisplayName + ":" + accountId;
        }

        private static int GenerateCode(byte[] securityToken, string modifier)
        {
            var currentTimestampNumber = GetCurrentTimestampNumber();
            using var hashAlgorithm = new HMACSHA1(securityToken);
            return ComputeTotp(hashAlgorithm, currentTimestampNumber, modifier);
        }

        private static ulong GetCurrentTimestampNumber()
        {
            var delta = DateTimeOffset.UtcNow - UnixEpoch;
            return (ulong)(delta.Ticks / Timestamp.Ticks);
        }

        private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestampNumber, string modifier)
        {
            const int mod = 1000000;

            var timestampAsBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestampNumber));
            var hash = hashAlgorithm.ComputeHash(ApplyModifier(timestampAsBytes, modifier));

            var offset = hash[^1] & 0xf;
            Debug.Assert(offset + 4 < hash.Length);
            var binaryCode = (hash[offset] & 0x7f) << 24
                             | (hash[offset + 1] & 0xff) << 16
                             | (hash[offset + 2] & 0xff) << 8
                             | (hash[offset + 3] & 0xff);

            return binaryCode % mod;
        }

        private static byte[] ApplyModifier(byte[] input, string modifier)
        {
            if (string.IsNullOrWhiteSpace(modifier))
                return input;

            var modifierBytes = Encoding.GetBytes(modifier);
            var combined = new byte[checked(input.Length + modifierBytes.Length)];
            Buffer.BlockCopy(input, 0, combined, 0, input.Length);
            Buffer.BlockCopy(modifierBytes, 0, combined, input.Length, modifierBytes.Length);

            return combined;
        }
    }
}