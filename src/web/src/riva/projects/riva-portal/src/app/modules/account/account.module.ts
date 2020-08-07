import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { FormModule } from 'riva-shared';
import { AccountRoutingModule } from './account-routing.module';
import { AccountServicesModule } from './account-services.module';
import { AccountComponent } from './pages/account/account.component';
import { RegistrationConfirmationComponent } from './pages/registration-confirmation/registration-confirmation.component';
import { RegistrationComponent } from './pages/registration/registration.component';
import { RequestPasswordResetEmailComponent } from './pages/request-password-reset-email/request-password-reset-email.component';
import { RequestRegistrationConfirmationEmailComponent } from './pages/request-registration-confirmation-email/request-registration-confirmation-email.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';

@NgModule({
    declarations: [
        AccountComponent,
        RegistrationComponent,
        RegistrationConfirmationComponent,
        RequestRegistrationConfirmationEmailComponent,
        RequestPasswordResetEmailComponent,
        ResetPasswordComponent
    ],
    imports: [
        CommonModule,
        RouterModule,
        ReactiveFormsModule,
        TranslateModule.forChild(),
        FormModule,
        AccountRoutingModule,
        AccountServicesModule
    ]
})
export class AccountModule {}
