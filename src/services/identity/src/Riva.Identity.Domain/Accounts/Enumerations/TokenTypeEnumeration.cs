using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Enumerations
{
    public class TokenTypeEnumeration : EnumerationBase
    {
        public static TokenTypeEnumeration AccountConfirmation => new TokenTypeEnumeration(1, nameof(AccountConfirmation));
        public static TokenTypeEnumeration PasswordReset => new TokenTypeEnumeration(2, nameof(PasswordReset));

        private TokenTypeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}