import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AuthCodeRedirectModule, LayoutModule, PageNotFoundModule } from 'riva-shared';
import { ContentLayoutRoutingModule } from './content-layout-routing.module';

@NgModule({
    imports: [CommonModule, ContentLayoutRoutingModule, LayoutModule, AuthCodeRedirectModule, PageNotFoundModule]
})
export class ContentLayoutModule {}
