using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Core.Enumerations
{
    public class AccountErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static AccountErrorCodeEnumeration NotFound => new AccountErrorCodeEnumeration(1, nameof(NotFound));
        public static AccountErrorCodeEnumeration EmailIsAlreadyTaken => new AccountErrorCodeEnumeration(2, nameof(EmailIsAlreadyTaken));
        public static AccountErrorCodeEnumeration UserRoleIsNotRemovable => new AccountErrorCodeEnumeration(3, nameof(UserRoleIsNotRemovable));
        public static AccountErrorCodeEnumeration IncorrectPassword => new AccountErrorCodeEnumeration(4, nameof(IncorrectPassword));
        public static AccountErrorCodeEnumeration PasswordIsNotSet => new AccountErrorCodeEnumeration(5, nameof(PasswordIsNotSet));
        public static AccountErrorCodeEnumeration PasswordAlreadySet => new AccountErrorCodeEnumeration(6, nameof(PasswordAlreadySet));
        public static AccountErrorCodeEnumeration NotConfirmed => new AccountErrorCodeEnumeration(7, nameof(NotConfirmed));
        public static AccountErrorCodeEnumeration AlreadyConfirmed => new AccountErrorCodeEnumeration(8, nameof(AlreadyConfirmed));
        public static AccountErrorCodeEnumeration ConfirmationCodeWasNotGenerated => new AccountErrorCodeEnumeration(9, nameof(ConfirmationCodeWasNotGenerated));
        public static AccountErrorCodeEnumeration IncorrectConfirmationCode => new AccountErrorCodeEnumeration(10, nameof(IncorrectConfirmationCode));
        public static AccountErrorCodeEnumeration ConfirmationCodeExpired => new AccountErrorCodeEnumeration(11, nameof(ConfirmationCodeExpired));

        private AccountErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}