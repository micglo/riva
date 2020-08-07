import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { UiSwitchModule } from 'ngx-ui-switch';
import { ConfirmationAlertModule } from './../confirmation-alert/confirmation-alert.module';
import { FormModule } from './../form/form.module';
import { AssignPasswordTabComponent } from './common/components/assign-password-tab/assign-password-tab.component';
import { ChangePasswordTabComponent } from './common/components/change-password-tab/change-password-tab.component';
import { GeneralTabComponent } from './common/components/general-tab/general-tab.component';
import { MyAccountServicesModule } from './my-account-services.module';
import { MyAccountComponent } from './pages/my-account/my-account.component';
import { ProfileComponent } from './pages/profile/profile.component';

@NgModule({
    declarations: [MyAccountComponent, ProfileComponent, GeneralTabComponent, AssignPasswordTabComponent, ChangePasswordTabComponent],
    imports: [
        CommonModule,
        RouterModule,
        ReactiveFormsModule,
        UiSwitchModule,
        NgSelectModule,
        TranslateModule.forChild(),
        FormModule,
        ConfirmationAlertModule,
        MyAccountServicesModule
    ]
})
export class MyAccountModule {}
