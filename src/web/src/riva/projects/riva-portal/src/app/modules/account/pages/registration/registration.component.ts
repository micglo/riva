import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { TranslationService } from 'riva-core';
import { FormService } from 'riva-shared';
import { RegistrationService } from './../../common/services/registration.service';

@Component({
    selector: 'app-registration',
    templateUrl: './registration.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [RegistrationService]
})
export class RegistrationComponent implements OnInit {
    private _form: FormGroup;
    private _passwordFieldName: string;

    public get formGroup(): FormGroup {
        return this._form;
    }

    public get formGroupControls(): { [key: string]: AbstractControl } {
        return this._form.controls;
    }

    public get minPasswordLength(): number {
        return this.registrationService.minPasswordLength;
    }

    public get maxPasswordLength(): number {
        return this.registrationService.maxPasswordLength;
    }

    public get maxEmailLength(): number {
        return this.registrationService.maxEmailLength;
    }

    public get passwordFieldName(): string {
        return this._passwordFieldName;
    }

    constructor(
        private registrationService: RegistrationService,
        private formService: FormService,
        private translationService: TranslationService
    ) {}

    public ngOnInit(): void {
        const passwordFieldNameKey = 'account.registration.password';
        this._form = this.registrationService.createForm();
        this._passwordFieldName = this.translationService.translate(passwordFieldNameKey);
    }

    public isControlTouchedAndDirty(abstractControl: AbstractControl): boolean {
        return this.formService.isControlTouchedAndDirty(abstractControl);
    }

    public onSubmit(): void {
        this.registrationService.submit(this._form);
    }
}
