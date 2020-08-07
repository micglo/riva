import { ModuleWithProviders, NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SpinnerService } from './spinner.service';

@NgModule({
    imports: [NgxSpinnerModule],
    exports: [NgxSpinnerModule]
})
export class SpinnerModule {
    static forRoot(): ModuleWithProviders<SpinnerModule> {
        return {
            ngModule: SpinnerModule,
            providers: [SpinnerService]
        };
    }

    static forChild(): ModuleWithProviders<SpinnerModule> {
        return {
            ngModule: SpinnerModule
        };
    }
}
