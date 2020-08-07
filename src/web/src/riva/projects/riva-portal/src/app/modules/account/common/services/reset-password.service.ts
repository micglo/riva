import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiError, ApiErrorDetail, HttpStatusCode, NotificationService, SpinnerService } from 'riva-core';
import { FormValidationService, mustMatch } from 'riva-shared';
import { ResetPasswordErrorCode } from './../enums/reset-password-error-code.enum';
import { ResetPassword } from './../models/reset-password.model';
import { AccountService } from './../services/account.service';

@Injectable()
export class ResetPasswordService {
    private _maxEmailLength = 256;
    private _minPasswordLength = 6;
    private _maxPasswordLength = 100;

    public get maxEmailLength(): number {
        return this._maxEmailLength;
    }

    public get minPasswordLength(): number {
        return this._minPasswordLength;
    }

    public get maxPasswordLength(): number {
        return this._maxPasswordLength;
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
        return this.fb.group(
            {
                email: new FormControl(null, [Validators.required, Validators.email, Validators.maxLength(this.maxEmailLength)]),
                code: new FormControl(null, [Validators.required]),
                password: new FormControl(null, [
                    Validators.required,
                    Validators.minLength(this._minPasswordLength),
                    Validators.maxLength(this._maxPasswordLength)
                ]),
                confirmPassword: new FormControl(null, [
                    Validators.required,
                    Validators.minLength(this._minPasswordLength),
                    Validators.maxLength(this._maxPasswordLength)
                ])
            },
            {
                validator: mustMatch('password', 'confirmPassword')
            }
        );
    }

    public submit(form: FormGroup): void {
        if (form.invalid) {
            this.formValidationService.validateAllFormFields(form);
        } else {
            this.spinnerService.show();
            const resetPassword = form.getRawValue() as ResetPassword;
            this.accountService.resetPassword(resetPassword).subscribe(
                () => {
                    this.spinnerService.hide();
                    const successMessageKey = 'account.resetPassword.submit.successMessage';
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
            case ResetPasswordErrorCode.NotConfirmed: {
                return 'account.resetPassword.submit.notConfirmedErrorMessage';
            }
            case ResetPasswordErrorCode.PasswordIsNotSet: {
                return 'account.resetPassword.submit.passwordIsNotSetErrorMessage';
            }
            case ResetPasswordErrorCode.ConfirmationCodeExpired: {
                return 'account.resetPassword.submit.confirmationCodeExpiredErrorMessage';
            }
            case ResetPasswordErrorCode.ConfirmationCodeWasNotGenerated: {
                return 'account.resetPassword.submit.confirmationCodeWasNotGeneratedErrorMessage';
            }
            case ResetPasswordErrorCode.IncorrectConfirmationCode: {
                return 'account.resetPassword.submit.incorrectConfirmationCodeErrorMessage';
            }
            case ResetPasswordErrorCode.NotFound: {
                return 'account.resetPassword.submit.notFoundErrorMessage';
            }
            default: {
                return 'application.error.somethingWentWrong';
            }
        }
    }
}
