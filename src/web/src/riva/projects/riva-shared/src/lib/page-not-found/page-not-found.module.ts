import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { LayoutModule } from './../layout/layout.module';
import { PageNotFoundComponent } from './page-not-found.component';

@NgModule({
    declarations: [PageNotFoundComponent],
    imports: [CommonModule, RouterModule, LayoutModule, TranslateModule.forChild()],
    exports: [PageNotFoundComponent]
})
export class PageNotFoundModule {}
