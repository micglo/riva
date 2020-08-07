import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpStatusCode, NotificationService, SpinnerService } from 'riva-core';
import { FormValidationService } from './../../../form/validation/services/form-validation.service';
import { mustMatch } from './../../../form/validation/validators/must-match.validator';
import { ChangePassword } from './../models/change-password.model';
import { AccountService } from './account.service';

@Injectable()
export class ChangePasswordTabService {
    private _minPasswordLength = 6;
    private _maxPasswordLength = 100;

    public get minPasswordLength(): number {
        return this._minPasswordLength;
    }

    public get maxPasswordLength(): number {
        return this._maxPasswordLength;
    }

    constructor(
        private fb: FormBuilder,
        private cdr: ChangeDetectorRef,
        private formValidationService: FormValidationService,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService,
        private accountService: AccountService
    ) {}

    public createForm(): FormGroup {
        return this.fb.group(
            {
                oldPassword: new FormControl(null, [
                    Validators.required,
                    Validators.minLength(this._minPasswordLength),
                    Validators.maxLength(this._maxPasswordLength)
                ]),
                newPassword: new FormControl(null, [
                    Validators.required,
                    Validators.minLength(this._minPasswordLength),
                    Validators.maxLength(this._maxPasswordLength)
                ]),
                confirmNewPassword: new FormControl(null, [
                    Validators.required,
                    Validators.minLength(this._minPasswordLength),
                    Validators.maxLength(this._maxPasswordLength)
                ])
            },
            {
                validator: mustMatch('newPassword', 'confirmNewPassword')
            }
        );
    }

    public submit(form: FormGroup, accountId: string): void {
        if (form.invalid) {
            this.formValidationService.validateAllFormFields(form);
        } else {
            this.spinnerService.show();
            const changePassword = form.getRawValue() as ChangePassword;
            this.accountService.changePassword(accountId, changePassword).subscribe(
                () => {
                    form.reset();
                    this.cdr.markForCheck();
                    this.spinnerService.hide();
                    const successMessageKey = 'myAccount.profile.tab.changePassword.submit.successMessage';
                    this.notificationService.success(successMessageKey);
                },
                (error: HttpErrorResponse) => {
                    this.spinnerService.hide();
                    if (error.status === HttpStatusCode.unprocessableEntity) {
                        const errorMessageKey = 'myAccount.profile.tab.changePassword.submit.incorrectOldPasswordErrorMessage';
                        this.notificationService.error(errorMessageKey);
                    }
                }
            );
        }
    }
}
