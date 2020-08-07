import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { AppConfigurationProvider, ConfigurationModule } from 'riva-core';

@NgModule({
    declarations: [],
    imports: [ConfigurationModule.forRoot()]
})
export class CoreModule {
    constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
        if (parentModule) {
            throw new Error('CoreModule is already loaded. Import it in the AppModule only');
        }
    }

    static forRoot(): ModuleWithProviders<CoreModule> {
        return {
            ngModule: CoreModule,
            providers: [AppConfigurationProvider]
        };
    }
}
