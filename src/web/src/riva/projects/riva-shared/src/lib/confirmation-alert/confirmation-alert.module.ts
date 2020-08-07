import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { ConfirmationAlertComponent } from './confirmation-alert.component';

@NgModule({
    declarations: [ConfirmationAlertComponent],
    imports: [CommonModule, TranslateModule.forChild(), SweetAlert2Module],
    exports: [ConfirmationAlertComponent]
})
export class ConfirmationAlertModule {}
