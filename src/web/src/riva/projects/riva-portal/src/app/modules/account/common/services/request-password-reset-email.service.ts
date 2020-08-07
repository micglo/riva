import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiError, ApiErrorDetail, HttpStatusCode, NotificationService, SpinnerService } from 'riva-core';
import { FormValidationService } from 'riva-shared';
import { RequestPasswordResetEmailErrorCode } from './../enums/request-password-reset-email-error-code.enum';
import { RequestPasswordResetEmail } from './../models/request-password-reset-email.model';
import { AccountService } from './account.service';

@Injectable()
export class RequestPasswordResetEmailService {
    private _maxEmailLength = 256;

    public get maxEmailLength(): number {
        return this._maxEmailLength;
    }

    constructor(
        private fb: FormBuilder,
        private router: Router,
        private formValidationService: FormValidationService,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService,
        private accountService: AccountService
    ) {}

    public createForm(): FormGroup {
        return this.fb.group({
            email: new FormControl(null, [Validators.required, Validators.email, Validators.maxLength(this.maxEmailLength)])
        });
    }

    public submit(form: FormGroup): void {
        if (form.invalid) {
            this.formValidationService.validateAllFormFields(form);
        } else {
            this.spinnerService.show();
            const requestPasswordResetEmail = form.getRawValue() as RequestPasswordResetEmail;
            this.accountService.requestPasswordResetToken(requestPasswordResetEmail).subscribe(
                () => {
                    this.spinnerService.hide();
                    const successMessageKey = 'account.requestPasswordResetEmail.submit.successMessage';
                    this.notificationService.success(successMessageKey);
                    this.router.navigate(['/']);
                },
                (error: HttpErrorResponse) => {
                    this.spinnerService.hide();
                    if (error.status === HttpStatusCode.unprocessableEntity) {
                        const apiError = error.error as ApiError;
                        if (apiError) {
                            const errorMessageKey = this.getNotificationErrorMessage(apiError.details);
                            this.notificationService.error(errorMessageKey);
                        }
                    }
                }
            );
        }
    }

    private getNotificationErrorMessage(apiErrorDetails: Array<ApiErrorDetail>): string {
        if (!apiErrorDetails || apiErrorDetails.length > 1 || apiErrorDetails.length === 0) {
            return 'application.error.somethingWentWrong';
        }
        const detail = apiErrorDetails[0];
        switch (detail.errorCode) {
            case RequestPasswordResetEmailErrorCode.NotConfirmed: {
                return 'account.requestPasswordResetEmail.submit.notConfirmedErrorMessage';
            }
            case RequestPasswordResetEmailErrorCode.NotFound: {
                return 'account.requestPasswordResetEmail.submit.notFoundErrorMessage';
            }
            case RequestPasswordResetEmailErrorCode.PasswordIsNotSet: {
                return 'account.requestPasswordResetEmail.submit.passwordIsNotSetErrorMessage';
            }
            default: {
                return 'application.error.somethingWentWrong';
            }
        }
    }
}
