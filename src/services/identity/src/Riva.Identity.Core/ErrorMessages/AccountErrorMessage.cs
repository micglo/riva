namespace Riva.Identity.Core.ErrorMessages
{
    public static class AccountErrorMessage
    {
        public const string NotFound = "Account is not found.";
        public const string EmailIsAlreadyTaken = "Email is already taken.";
        public const string UserRoleIsNotRemovable = "Role with name 'User' is can not be removed.";
        public const string IncorrectPassword = "Incorrect password.";
        public const string PasswordIsNotSet = "Password is not set.";
        public const string PasswordAlreadySet = "Password is already set.";
        public const string NotConfirmed = "Account is not confirmed.";
        public const string AlreadyConfirmed = "Account is already confirmed.";
        public const string ConfirmationCodeWasNotGenerated = "Confirmation code was not generated.";
        public const string IncorrectConfirmationCode = "Incorrect confirmation code.";
        public const string ConfirmationCodeExpired = "Confirmation code is expired.";
    }
}