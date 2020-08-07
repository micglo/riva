import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { LayoutModule } from 'riva-shared';
import { FullLayoutRoutingModule } from './full-layout-routing.module';
import { FullLayoutComponent } from './pages/full-layout/full-layout.component';

@NgModule({
    declarations: [FullLayoutComponent],
    imports: [CommonModule, FullLayoutRoutingModule, LayoutModule]
})
export class FullLayoutModule {}
