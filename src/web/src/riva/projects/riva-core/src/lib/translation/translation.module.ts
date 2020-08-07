import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { TRANSLATION_OPTIONS_PROVIDER } from './translation-options.provider';
import { TranslationService } from './translation.service';

@NgModule()
export class TranslationModule {
    constructor(@Optional() @SkipSelf() parentModule: TranslationModule) {
        if (parentModule) {
            throw new Error('TranslationModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<TranslationModule> {
        return {
            ngModule: TranslationModule,
            providers: [TranslationService, TRANSLATION_OPTIONS_PROVIDER]
        };
    }
}
