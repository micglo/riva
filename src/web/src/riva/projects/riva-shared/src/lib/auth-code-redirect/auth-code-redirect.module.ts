import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { LayoutModule } from './../layout/layout.module';
import { AuthCodeRedirectComponent } from './auth-code-redirect.component';

@NgModule({
    declarations: [AuthCodeRedirectComponent],
    imports: [CommonModule, LayoutModule]
})
export class AuthCodeRedirectModule {}
