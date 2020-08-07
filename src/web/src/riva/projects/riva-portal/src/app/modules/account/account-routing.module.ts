import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PreventLoggedInGuard } from 'riva-core';
import { AccountComponent } from './pages/account/account.component';
import { RegistrationConfirmationComponent } from './pages/registration-confirmation/registration-confirmation.component';
import { RegistrationComponent } from './pages/registration/registration.component';
import { RequestPasswordResetEmailComponent } from './pages/request-password-reset-email/request-password-reset-email.component';
import { RequestRegistrationConfirmationEmailComponent } from './pages/request-registration-confirmation-email/request-registration-confirmation-email.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';

const routes: Routes = [
    {
        path: '',
        component: AccountComponent,
        canActivate: [PreventLoggedInGuard],
        canActivateChild: [PreventLoggedInGuard],
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'registration'
            },
            { path: 'registration', component: RegistrationComponent },
            { path: 'registration-confirmation', component: RegistrationConfirmationComponent },
            { path: 'request-registration-confirmation-email', component: RequestRegistrationConfirmationEmailComponent },
            { path: 'request-password-reset-email', component: RequestPasswordResetEmailComponent },
            { path: 'reset-password', component: ResetPasswordComponent }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AccountRoutingModule {}
