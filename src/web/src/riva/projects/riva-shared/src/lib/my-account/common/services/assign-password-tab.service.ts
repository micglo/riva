import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NotificationService, SpinnerService } from 'riva-core';
import { BehaviorSubject, Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { FormValidationService } from './../../../form/validation/services/form-validation.service';
import { mustMatch } from './../../../form/validation/validators/must-match.validator';
import { AssignPassword } from './../models/assign-password.model';
import { MyAccount } from './../models/my-account.model';
import { MyAccountStore } from './../stores/my-account.store';
import { AccountService } from './account.service';

@Injectable()
export class AssignPasswordTabService {
    private _passwordAssignedSubject$ = new BehaviorSubject<boolean>(null);
    private _minPasswordLength = 6;
    private _maxPasswordLength = 100;

    public get passwordAssigned$(): Observable<boolean> {
        return this._passwordAssignedSubject$.asObservable();
    }

    public get minPasswordLength(): number {
        return this._minPasswordLength;
    }

    public get maxPasswordLength(): number {
        return this._maxPasswordLength;
    }

    constructor(
        private fb: FormBuilder,
        private formValidationService: FormValidationService,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService,
        private accountService: AccountService,
        private myAccountStore: MyAccountStore
    ) {}

    public createForm(): FormGroup {
        return this.fb.group(
            {
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

    public submit(form: FormGroup, accountId: string): void {
        if (form.invalid) {
            this.formValidationService.validateAllFormFields(form);
            this._passwordAssignedSubject$.next(false);
        } else {
            this.spinnerService.show();
            const assignPassword = form.getRawValue() as AssignPassword;
            this.accountService
                .assignPassword(accountId, assignPassword)
                .pipe(switchMap(() => this.accountService.getById(accountId)))
                .subscribe(
                    (myAccount: MyAccount) => {
                        this.myAccountStore.updateState(myAccount);
                        this._passwordAssignedSubject$.next(true);
                        this.spinnerService.hide();
                        const successMessageKey = 'myAccount.profile.tab.assignPassword.submit.successMessage';
                        this.notificationService.success(successMessageKey);
                    },
                    () => this.spinnerService.hide()
                );
        }
    }
}
