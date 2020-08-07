import { ChangeDetectionStrategy, Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { TranslationService } from 'riva-core';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { FormService } from './../../../../form/services/form.service';
import { MyAccount } from './../../models/my-account.model';
import { AssignPasswordTabService } from './../../services/assign-password-tab.service';
import { MyAccountStore } from './../../stores/my-account.store';

@Component({
    selector: 'lib-assign-password-tab',
    templateUrl: './assign-password-tab.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [AssignPasswordTabService]
})
export class AssignPasswordTabComponent implements OnInit, OnDestroy {
    private _form: FormGroup;
    private _passwordFieldName: string;
    private _myAccount: MyAccount;
    private _subscription = new Subscription();

    @Output() public passwordAssigned = new EventEmitter();

    public get formGroup(): FormGroup {
        return this._form;
    }

    public get formGroupControls(): { [key: string]: AbstractControl } {
        return this._form.controls;
    }

    public get minPasswordLength(): number {
        return this.assignPasswordTabService.minPasswordLength;
    }

    public get maxPasswordLength(): number {
        return this.assignPasswordTabService.maxPasswordLength;
    }

    public get passwordFieldName(): string {
        return this._passwordFieldName;
    }

    constructor(
        private assignPasswordTabService: AssignPasswordTabService,
        private myAccountStore: MyAccountStore,
        private formService: FormService,
        private translationService: TranslationService
    ) {}

    public ngOnInit(): void {
        const passwordFieldNameKey = 'myAccount.profile.tab.assignPassword.password';
        this._form = this.assignPasswordTabService.createForm();
        this._passwordFieldName = this.translationService.translate(passwordFieldNameKey);
        this.setPasswordFieldNameOnLanguageChange(passwordFieldNameKey);
        this.loadMyAccount();
        this.emitPasswordAssignedEventOnPasswordAssignedChange();
    }

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    public isControlTouchedAndDirty(abstractControl: AbstractControl): boolean {
        return this.formService.isControlTouchedAndDirty(abstractControl);
    }

    public onSubmit(): void {
        this.assignPasswordTabService.submit(this._form, this._myAccount.id);
    }

    private setPasswordFieldNameOnLanguageChange(passwordFieldNameKey: string): void {
        const langChangeSubscription = this.translationService.onLanguageChange.subscribe(
            () => (this._passwordFieldName = this.translationService.translate(passwordFieldNameKey))
        );
        this._subscription.add(langChangeSubscription);
    }

    private loadMyAccount(): void {
        const loadMyAccountSubscription = this.myAccountStore.state$.subscribe((myAccount: MyAccount) => (this._myAccount = myAccount));
        this._subscription.add(loadMyAccountSubscription);
    }

    private emitPasswordAssignedEventOnPasswordAssignedChange(): void {
        const passwordAssignedSubscription = this.assignPasswordTabService.passwordAssigned$
            .pipe(filter((passwordAssigned: boolean) => passwordAssigned))
            .subscribe(() => this.passwordAssigned.emit());
        this._subscription.add(passwordAssignedSubscription);
    }
}
