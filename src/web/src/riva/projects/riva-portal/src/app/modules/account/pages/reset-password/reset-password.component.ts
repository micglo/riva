import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { TranslationService } from 'riva-core';
import { FormService } from 'riva-shared';
import { filter } from 'rxjs/operators';
import { ResetPasswordService } from './../../common/services/reset-password.service';

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [ResetPasswordService]
})
export class ResetPasswordComponent implements OnInit {
    private _form: FormGroup;
    private _passwordFieldName: string;

    public get formGroup(): FormGroup {
        return this._form;
    }

    public get formGroupControls(): { [key: string]: AbstractControl } {
        return this._form.controls;
    }

    public get maxEmailLength(): number {
        return this.resetPasswordService.maxEmailLength;
    }

    public get minPasswordLength(): number {
        return this.resetPasswordService.minPasswordLength;
    }

    public get maxPasswordLength(): number {
        return this.resetPasswordService.maxPasswordLength;
    }

    public get passwordFieldName(): string {
        return this._passwordFieldName;
    }

    constructor(
        private route: ActivatedRoute,
        private resetPasswordService: ResetPasswordService,
        private formService: FormService,
        private translationService: TranslationService
    ) {}

    public ngOnInit(): void {
        const passwordFieldNameKey = 'account.resetPassword.password';
        this._form = this.resetPasswordService.createForm();
        this._passwordFieldName = this.translationService.translate(passwordFieldNameKey);
        this.route.queryParams.pipe(filter((params: Params) => params.email && params.code)).subscribe((params: Params) => {
            this._form.patchValue({ email: params.email, code: params.code });
        });
    }

    public isControlTouchedAndDirty(abstractControl: AbstractControl): boolean {
        return this.formService.isControlTouchedAndDirty(abstractControl);
    }

    public onSubmit(): void {
        this.resetPasswordService.submit(this._form);
    }
}
