export enum ResetPasswordErrorCode {
    NotFound = 'NotFound',
    NotConfirmed = 'NotConfirmed',
    PasswordIsNotSet = 'PasswordIsNotSet',
    ConfirmationCodeWasNotGenerated = 'ConfirmationCodeWasNotGenerated',
    IncorrectConfirmationCode = 'IncorrectConfirmationCode',
    ConfirmationCodeExpired = 'ConfirmationCodeExpired'
}
