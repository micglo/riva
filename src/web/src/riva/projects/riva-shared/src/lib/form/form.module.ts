import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { FormServicesModule } from './form-services.module';
import { EmailErrorComponent } from './validation/components/email-error/email-error.component';
import { MaxLengthErrorComponent } from './validation/components/max-length-error/max-length-error.component';
import { MinLengthErrorComponent } from './validation/components/min-length-error/min-length-error.component';
import { MinValueErrorComponent } from './validation/components/min-value-error/min-value-error.component';
import { MustMatchErrorComponent } from './validation/components/must-match-error/must-match-error.component';
import { RequiredErrorComponent } from './validation/components/required-error/required-error.component';

@NgModule({
    declarations: [
        EmailErrorComponent,
        RequiredErrorComponent,
        MaxLengthErrorComponent,
        MinLengthErrorComponent,
        MustMatchErrorComponent,
        MinValueErrorComponent
    ],
    exports: [
        EmailErrorComponent,
        RequiredErrorComponent,
        MaxLengthErrorComponent,
        MinLengthErrorComponent,
        MustMatchErrorComponent,
        MinValueErrorComponent
    ],
    imports: [CommonModule, TranslateModule.forChild(), FormServicesModule]
})
export class FormModule {}
