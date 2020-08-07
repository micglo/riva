import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { APP_CONFIGURATION_PROVIDER } from './app-configuration.provider';
import { AppConfigurationService } from './app-configuration.service';
import { AppInitService } from './app-init.service';

@NgModule()
export class ConfigurationModule {
    constructor(@Optional() @SkipSelf() parentModule: ConfigurationModule) {
        if (parentModule) {
            throw new Error('ConfigurationModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<ConfigurationModule> {
        return {
            ngModule: ConfigurationModule,
            providers: [AppConfigurationService, AppInitService, APP_CONFIGURATION_PROVIDER]
        };
    }
}
