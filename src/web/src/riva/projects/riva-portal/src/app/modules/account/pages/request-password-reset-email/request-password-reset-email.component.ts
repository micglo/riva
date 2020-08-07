import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { FormService } from 'riva-shared';
import { RequestPasswordResetEmailService } from './../../common/services/request-password-reset-email.service';

@Component({
    selector: 'app-request-password-reset-email',
    templateUrl: './request-password-reset-email.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [RequestPasswordResetEmailService]
})
export class RequestPasswordResetEmailComponent implements OnInit {
    private _form: FormGroup;

    public get formGroup(): FormGroup {
        return this._form;
    }

    public get formGroupControls(): { [key: string]: AbstractControl } {
        return this._form.controls;
    }

    public get maxEmailLength(): number {
        return this.requestPasswordResetEmailService.maxEmailLength;
    }

    constructor(private requestPasswordResetEmailService: RequestPasswordResetEmailService, private formService: FormService) {}

    public ngOnInit(): void {
        this._form = this.requestPasswordResetEmailService.createForm();
    }

    public isControlTouchedAndDirty(abstractControl: AbstractControl): boolean {
        return this.formService.isControlTouchedAndDirty(abstractControl);
    }

    public onSubmit(): void {
        this.requestPasswordResetEmailService.submit(this._form);
    }
}
