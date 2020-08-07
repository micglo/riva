import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoggedInGuard } from 'riva-core';
import { MyAccountComponent, ProfileComponent } from 'riva-shared';

const routes: Routes = [
    {
        path: '',
        component: MyAccountComponent,
        canActivate: [LoggedInGuard],
        canActivateChild: [LoggedInGuard],
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'profile'
            },
            { path: 'profile', component: ProfileComponent }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MyAccountRoutingModule {}
