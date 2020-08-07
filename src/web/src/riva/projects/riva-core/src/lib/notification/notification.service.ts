import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { TranslationService } from './../translation/translation.service';

@Injectable()
export class NotificationService {
    constructor(private toastr: ToastrService, private translationService: TranslationService) {}

    public error(messageTranslationKey: string): void {
        this.toastr.error(this.translationService.translate(messageTranslationKey));
    }

    public success(messageTranslationKey: string): void {
        this.toastr.success(this.translationService.translate(messageTranslationKey));
    }
}
