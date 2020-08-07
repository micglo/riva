import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { FormService } from 'riva-shared';
import { RequestRegistrationConfirmationEmailService } from './../../common/services/request-registration-confirmation-email.service';

@Component({
    selector: 'app-request-registration-confirmation-email',
    templateUrl: './request-registration-confirmation-email.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [RequestRegistrationConfirmationEmailService]
})
export class RequestRegistrationConfirmationEmailComponent implements OnInit {
    private _form: FormGroup;

    public get formGroup(): FormGroup {
        return this._form;
    }

    public get formGroupControls(): { [key: string]: AbstractControl } {
        return this._form.controls;
    }

    public get maxEmailLength(): number {
        return this.requestRegistrationConfirmationEmailService.maxEmailLength;
    }

    constructor(
        private requestRegistrationConfirmationEmailService: RequestRegistrationConfirmationEmailService,
        private formService: FormService
    ) {}

    public ngOnInit(): void {
        this._form = this.requestRegistrationConfirmationEmailService.createForm();
    }

    public isControlTouchedAndDirty(abstractControl: AbstractControl): boolean {
        return this.formService.isControlTouchedAndDirty(abstractControl);
    }

    public onSubmit(): void {
        this.requestRegistrationConfirmationEmailService.submit(this._form);
    }
}
