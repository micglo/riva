import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ApiError, ApiErrorDetail, HttpStatusCode, NotificationService } from 'riva-core';
import { RegistrationConfirmationErrorCode } from './../enums/registration-confirmation-error-code.enum';

@Injectable()
export class RegistrationConfirmationService {
    constructor(private router: Router, private notificationService: NotificationService) {}

    public displaySuccessMessageAndRedirectToHome(): void {
        const successMessageKey = 'account.registrationConfirmation.confirmAccount.successMessage';
        this.notificationService.success(successMessageKey);
        this.router.navigate(['/home']);
    }

    public disaplyErrorMessageAndRedirectToHome(error: HttpErrorResponse): void {
        if (error.status === HttpStatusCode.unprocessableEntity) {
            const apiError = error.error as ApiError;
            if (apiError) {
                const errorMessageKey = this.getNotificationErrorMessage(apiError.details);
                this.notificationService.error(errorMessageKey);
            }
        }
        this.router.navigate(['/home']);
    }

    private getNotificationErrorMessage(apiErrorDetails: Array<ApiErrorDetail>): string {
        if (!apiErrorDetails || apiErrorDetails.length > 1 || apiErrorDetails.length === 0) {
            return 'application.error.somethingWentWrong';
        }
        const detail = apiErrorDetails[0];
        switch (detail.errorCode) {
            case RegistrationConfirmationErrorCode.AlreadyConfirmed: {
                return 'account.registrationConfirmation.confirmAccount.alreadyConfirmedErrorMessage';
            }
            case RegistrationConfirmationErrorCode.ConfirmationCodeExpired: {
                return 'account.registrationConfirmation.confirmAccount.confirmationCodeExpiredErrorMessage';
            }
            case RegistrationConfirmationErrorCode.ConfirmationCodeWasNotGenerated: {
                return 'account.registrationConfirmation.confirmAccount.confirmationCodeWasNotGeneratedErrorMessage';
            }
            case RegistrationConfirmationErrorCode.IncorrectConfirmationCode: {
                return 'account.registrationConfirmation.confirmAccount.incorrectConfirmationCodeErrorMessage';
            }
            case RegistrationConfirmationErrorCode.NotFound: {
                return 'account.registrationConfirmation.confirmAccount.notFoundErrorMessage';
            }
            default: {
                return 'application.error.somethingWentWrong';
            }
        }
    }
}
