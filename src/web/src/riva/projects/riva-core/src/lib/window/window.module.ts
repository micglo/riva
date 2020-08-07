import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { BROWSER_WINDOW_REFERENCE_PROVIDER } from './browser-window-reference.provider';
import { WINDOW_PROVIDER } from './window.provider';

@NgModule()
export class WindowModule {
    constructor(@Optional() @SkipSelf() parentModule: WindowModule) {
        if (parentModule) {
            throw new Error('WindowModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<WindowModule> {
        return {
            ngModule: WindowModule,
            providers: [BROWSER_WINDOW_REFERENCE_PROVIDER, WINDOW_PROVIDER]
        };
    }
}
