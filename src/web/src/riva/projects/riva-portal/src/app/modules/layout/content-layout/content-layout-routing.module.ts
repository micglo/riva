import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthCodeRedirectComponent, ContentLayoutComponent, PageNotFoundComponent } from 'riva-shared';

const routes: Routes = [
    {
        path: '',
        component: ContentLayoutComponent,
        children: [
            { path: 'index.html', component: AuthCodeRedirectComponent },
            {
                path: 'account',
                loadChildren: () => import('./../../account/account.module').then(m => m.AccountModule)
            },
            { path: '**', component: PageNotFoundComponent }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ContentLayoutRoutingModule {}
