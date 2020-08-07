import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'home'
    },
    {
        path: '',
        loadChildren: () => import('./modules/layout/full-layout/full-layout.module').then(m => m.FullLayoutModule)
    },
    {
        path: '',
        loadChildren: () => import('./modules/layout/content-layout/content-layout.module').then(m => m.ContentLayoutModule)
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}
