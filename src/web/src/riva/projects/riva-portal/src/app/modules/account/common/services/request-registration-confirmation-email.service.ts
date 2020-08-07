import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiError, ApiErrorDetail, HttpStatusCode, NotificationService, SpinnerService } from 'riva-core';
import { FormValidationService } from 'riva-shared';
import { RequestRegistrationConfirmationEmailErrorCode } from './../enums/request-registration-confirmation-email-error-code.enum';
import { RequestRegistrationConfirmationEmail } from './../models/request-registration-confirmation-email.model';
import { AccountService } from './account.service';

@Injectable()
export class RequestRegistrationConfirmationEmailService {
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
            const requestRegistrationConfirmationEmail = form.getRawValue() as RequestRegistrationConfirmationEmail;
            this.accountService.requestAccountConfirmationToken(requestRegistrationConfirmationEmail).subscribe(
                () => {
                    this.spinnerService.hide();
                    const successMessageKey = 'account.requestRegistrationConfirmationEmail.submit.successMessage';
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
            case RequestRegistrationConfirmationEmailErrorCode.AlreadyConfirmed: {
                return 'account.requestRegistrationConfirmationEmail.submit.alreadyConfirmedErrorMessage';
            }
            case RequestRegistrationConfirmationEmailErrorCode.NotFound: {
                return 'account.requestRegistrationConfirmationEmail.submit.notFoundErrorMessage';
            }
            default: {
                return 'application.error.somethingWentWrong';
            }
        }
    }
}
