export enum RegistrationConfirmationErrorCode {
    NotFound = 'NotFound',
    AlreadyConfirmed = 'AlreadyConfirmed',
    ConfirmationCodeWasNotGenerated = 'ConfirmationCodeWasNotGenerated',
    IncorrectConfirmationCode = 'IncorrectConfirmationCode',
    ConfirmationCodeExpired = 'ConfirmationCodeExpired'
}
