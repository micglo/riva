import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { PERFECT_SCROLLBAR_CONFIG_PROVIDER } from './perfect-scrollbar-config.provider';

@NgModule()
export class PerfectScrollbarConfigModule {
    constructor(@Optional() @SkipSelf() parentModule: PerfectScrollbarConfigModule) {
        if (parentModule) {
            throw new Error('PerfectScrollbarConfigModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<PerfectScrollbarConfigModule> {
        return {
            ngModule: PerfectScrollbarConfigModule,
            providers: [PERFECT_SCROLLBAR_CONFIG_PROVIDER]
        };
    }
}
