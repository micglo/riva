using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Core.Enumerations
{
    public class AccountErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static AccountErrorCodeEnumeration NotFound => new AccountErrorCodeEnumeration(1, nameof(NotFound));
        public static AccountErrorCodeEnumeration EmailMismatch => new AccountErrorCodeEnumeration(2, nameof(EmailMismatch));

        private AccountErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}