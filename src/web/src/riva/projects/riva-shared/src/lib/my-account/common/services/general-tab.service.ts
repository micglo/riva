import { ChangeDetectorRef, Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import {
    AnnouncementSendingFrequency,
    NotificationService,
    SpinnerService,
    TranslationService,
    UpdateUser,
    User,
    UserService
} from 'riva-core';
import { FormValidationService } from './../../../form/validation/services/form-validation.service';
import { AVATAR_URL } from './../../../layout/constants/images.const';
import { AnnouncementSendingFrequencySelect } from './../models/announcement-sending-frequency-select.model';
import { AccountService } from './account.service';

@Injectable()
export class GeneralTabService {
    private _maxEmailLength = 256;
    private _minAnouncementPreferenceLimitValue = 1;

    public get maxEmailLength(): number {
        return this._maxEmailLength;
    }

    public get minAnouncementPreferenceLimitValue(): number {
        return this._minAnouncementPreferenceLimitValue;
    }

    constructor(
        private fb: FormBuilder,
        private cdr: ChangeDetectorRef,
        private translationService: TranslationService,
        private formValidationService: FormValidationService,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService,
        private userService: UserService,
        private accountService: AccountService
    ) {}

    public createForm(): FormGroup {
        return this.fb.group({
            id: new FormControl(null),
            email: new FormControl({ value: null, disabled: true }, [
                Validators.required,
                Validators.email,
                Validators.maxLength(this.maxEmailLength)
            ]),
            serviceActive: [false],
            announcementPreferenceLimit: new FormControl(null, [
                Validators.required,
                Validators.min(this._minAnouncementPreferenceLimitValue)
            ]),
            announcementSendingFrequency: new FormControl(null, [Validators.required]),
            picture: null
        });
    }

    public setFormValue(form: FormGroup, user: User): void {
        const formValue = {
            id: user.id,
            email: user.email,
            serviceActive: user.serviceActive,
            announcementPreferenceLimit: user.announcementPreferenceLimit,
            announcementSendingFrequency: user.announcementSendingFrequency,
            picture: null
        };
        form.setValue(formValue);
        this.cdr.markForCheck();
    }

    public setImage(form: FormGroup, file: File): void {
        form.get('picture').patchValue(file);
        this.cdr.markForCheck();
    }

    public getUserPictureUrl(user: User): string {
        return user.picture ? user.picture : AVATAR_URL;
    }

    public createAnnouncementSendingFrequencies(): Array<AnnouncementSendingFrequencySelect> {
        return Object.entries(AnnouncementSendingFrequency).map((value: [string, AnnouncementSendingFrequency]) => ({
            value: this.getAnnouncementSendingFrequencyKeyByValue(value[1]),
            label: this.translationService.translate(AnnouncementSendingFrequency[value[0]])
        }));
    }

    public submit(form: FormGroup): void {
        if (form.invalid) {
            this.formValidationService.validateAllFormFields(form);
        } else {
            this.spinnerService.show();
            const updateUser = form.getRawValue() as UpdateUser;
            this.userService.update(updateUser).subscribe(
                (user: User) => {
                    form.reset();
                    this.setFormValue(form, user);
                    this.cdr.markForCheck();
                    this.spinnerService.hide();
                    const successMessageKey = 'myAccount.profile.tab.general.submit.successMessage';
                    this.notificationService.success(successMessageKey);
                },
                () => this.spinnerService.hide()
            );
        }
    }

    public deleteAccount(id: string): void {
        this.spinnerService.show();
        this.accountService.delete(id).subscribe(
            () => {
                this.spinnerService.hide();
                const successMessageKey = 'myAccount.profile.tab.general.deleteAccount.successMessage';
                this.notificationService.success(successMessageKey);
            },
            () => this.spinnerService.hide()
        );
    }

    private getAnnouncementSendingFrequencyKeyByValue(value: string): AnnouncementSendingFrequency {
        const keys = Object.keys(AnnouncementSendingFrequency).filter(x => AnnouncementSendingFrequency[x] === value);
        return keys[0] as AnnouncementSendingFrequency;
    }
}
