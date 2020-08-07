import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpStatusCode, NotificationService, SpinnerService } from 'riva-core';
import { FormValidationService, mustMatch } from 'riva-shared';
import { CreateNewAccount } from './../models/create-new-account.model';
import { AccountService } from './account.service';

@Injectable()
export class RegistrationService {
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
                password: new FormControl(null, [
                    Validators.required,
                    Validators.minLength(this._minPasswordLength),
                    Validators.maxLength(this._maxPasswordLength)
                ]),
                confirmPassword: new FormControl(null, [
                    Validators.required,
                    Validators.minLength(this._minPasswordLength),
                    Validators.maxLength(this._maxPasswordLength)
                ]),
                acceptTerms: new FormControl(false, [Validators.requiredTrue])
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
            const createNewAccount = form.getRawValue() as CreateNewAccount;
            this.accountService.createAccount(createNewAccount).subscribe(
                () => {
                    this.spinnerService.hide();
                    const successMessageKey = 'account.registration.submit.successMessage';
                    this.notificationService.success(successMessageKey);
                    this.router.navigate(['/']);
                },
                (error: HttpErrorResponse) => {
                    this.spinnerService.hide();
                    if (error.status === HttpStatusCode.conflict) {
                        const errorMessageKey = 'account.registration.submit.emailIsAlreadyTakenErrorMessage';
                        this.notificationService.error(errorMessageKey);
                    }
                }
            );
        }
    }
}
