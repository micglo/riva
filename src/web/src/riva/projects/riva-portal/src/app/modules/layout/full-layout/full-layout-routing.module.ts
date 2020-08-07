import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FullLayoutComponent } from './pages/full-layout/full-layout.component';

const routes: Routes = [
    {
        path: '',
        component: FullLayoutComponent,
        children: [
            {
                path: 'home',
                loadChildren: () => import('./../../home/home.module').then(m => m.HomeModule)
            },
            {
                path: 'my-account',
                loadChildren: () => import('./../../my-account/my-account.module').then(m => m.MyAccountModule)
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FullLayoutRoutingModule {}
