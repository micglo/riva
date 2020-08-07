import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { TranslationService } from 'riva-core';
import { Subscription } from 'rxjs';
import { FormService } from './../../../../form/services/form.service';
import { MyAccount } from './../../models/my-account.model';
import { ChangePasswordTabService } from './../../services/change-password-tab.service';
import { MyAccountStore } from './../../stores/my-account.store';

@Component({
    selector: 'lib-change-password-tab',
    templateUrl: './change-password-tab.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [ChangePasswordTabService]
})
export class ChangePasswordTabComponent implements OnInit, OnDestroy {
    private _form: FormGroup;
    private _newPasswordFieldName: string;
    private _myAccount: MyAccount;
    private _subscription = new Subscription();

    public get formGroup(): FormGroup {
        return this._form;
    }

    public get formGroupControls(): { [key: string]: AbstractControl } {
        return this._form.controls;
    }

    public get minPasswordLength(): number {
        return this.changePasswordTabService.minPasswordLength;
    }

    public get maxPasswordLength(): number {
        return this.changePasswordTabService.maxPasswordLength;
    }

    public get newPasswordFieldName(): string {
        return this._newPasswordFieldName;
    }

    constructor(
        private changePasswordTabService: ChangePasswordTabService,
        private myAccountStore: MyAccountStore,
        private formService: FormService,
        private translationService: TranslationService
    ) {}

    public ngOnInit(): void {
        const newPasswordFieldNameKey = 'myAccount.profile.tab.changePassword.newPassword';
        this._form = this.changePasswordTabService.createForm();
        this._newPasswordFieldName = this.translationService.translate(newPasswordFieldNameKey);
        this.setNewPasswordFieldNameOnLanguageChange(newPasswordFieldNameKey);
        this.loadMyAccount();
    }

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    public isControlTouchedAndDirty(abstractControl: AbstractControl): boolean {
        return this.formService.isControlTouchedAndDirty(abstractControl);
    }

    public onSubmit(): void {
        this.changePasswordTabService.submit(this._form, this._myAccount.id);
    }

    private setNewPasswordFieldNameOnLanguageChange(newPasswordFieldNameKey: string): void {
        const langChangeSubscription = this.translationService.onLanguageChange.subscribe(
            () => (this._newPasswordFieldName = this.translationService.translate(newPasswordFieldNameKey))
        );
        this._subscription.add(langChangeSubscription);
    }

    private loadMyAccount(): void {
        const loadMyAccountSubscription = this.myAccountStore.state$.subscribe((myAccount: MyAccount) => (this._myAccount = myAccount));
        this._subscription.add(loadMyAccountSubscription);
    }
}
