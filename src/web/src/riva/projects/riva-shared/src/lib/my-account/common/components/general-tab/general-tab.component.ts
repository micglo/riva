import { ChangeDetectionStrategy, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { AuthService, TranslationService, User, UserService } from 'riva-core';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { ConfirmationAlertComponent } from './../../../../confirmation-alert/confirmation-alert.component';
import { Icon } from './../../../../confirmation-alert/icon.enum';
import { FormService } from './../../../../form/services/form.service';
import { AnnouncementSendingFrequencySelect } from './../../models/announcement-sending-frequency-select.model';
import { GeneralTabService } from './../../services/general-tab.service';

@Component({
    selector: 'lib-general-tab',
    templateUrl: './general-tab.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [GeneralTabService]
})
export class GeneralTabComponent implements OnInit, OnDestroy {
    private _form: FormGroup;
    private _user: User;
    private _pictureUrl: string;
    private _subscription = new Subscription();
    private _announcementSendingFrequencies = new Array<AnnouncementSendingFrequencySelect>();

    @ViewChild('confirmationAltert') public confirmationAltert: ConfirmationAlertComponent;
    public icon = Icon;

    public get formGroup(): FormGroup {
        return this._form;
    }

    public get formGroupControls(): { [key: string]: AbstractControl } {
        return this._form.controls;
    }

    public get pictureUrl(): string {
        return this._pictureUrl;
    }

    public get maxEmailLength(): number {
        return this.generalTabService.maxEmailLength;
    }

    public get minAnouncementPreferenceLimitValue(): number {
        return this.generalTabService.minAnouncementPreferenceLimitValue;
    }

    public get announcementSendingFrequencies(): Array<AnnouncementSendingFrequencySelect> {
        return this._announcementSendingFrequencies;
    }

    public get isAdmin(): boolean {
        return this.authService.isAdmin;
    }

    constructor(
        private generalTabService: GeneralTabService,
        private formService: FormService,
        private userService: UserService,
        private translationService: TranslationService,
        private authService: AuthService
    ) {}

    public ngOnInit(): void {
        this._form = this.generalTabService.createForm();
        this._announcementSendingFrequencies = this.generalTabService.createAnnouncementSendingFrequencies();
        this.setAnnouncementSendingFrequenciesOnLanguageChange();
        this.loadUser(this._form);
    }

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    public isControlTouchedAndDirty(abstractControl: AbstractControl): boolean {
        return this.formService.isControlTouchedAndDirty(abstractControl);
    }

    public onSubmit(): void {
        this.generalTabService.submit(this._form);
    }

    public fileChange(event: Event): void {
        const targetfiles = (event.target as HTMLInputElement).files;
        if (targetfiles && targetfiles.length) {
            const file = targetfiles[0];
            const reader = new FileReader();
            reader.onload = (e: ProgressEvent<FileReader>) => {
                this._pictureUrl = e.target.result as string;
                this.generalTabService.setImage(this._form, file);
            };
            reader.readAsDataURL(file);
        }
    }

    public resetImage(): void {
        this._pictureUrl = this.generalTabService.getUserPictureUrl(this._user);
        this.generalTabService.setImage(this._form, null);
    }

    public confirmAccountDeletion(): void {
        this.confirmationAltert.fire();
    }

    public onConfirmed(event: boolean): void {
        if (event) {
            this.generalTabService.deleteAccount(this._form.controls.id.value);
        }
    }

    private setAnnouncementSendingFrequenciesOnLanguageChange(): void {
        const langChangeSubscription = this.translationService.onLanguageChange.subscribe(
            () => (this._announcementSendingFrequencies = this.generalTabService.createAnnouncementSendingFrequencies())
        );
        this._subscription.add(langChangeSubscription);
    }

    private loadUser(form: FormGroup): void {
        const loadUserSubscription = this.userService.user$.pipe(filter((user: User) => user !== null)).subscribe((user: User) => {
            this._user = user;
            this._pictureUrl = this.generalTabService.getUserPictureUrl(this._user);
            this.generalTabService.setFormValue(form, user);
        });
        this._subscription.add(loadUserSubscription);
    }
}
