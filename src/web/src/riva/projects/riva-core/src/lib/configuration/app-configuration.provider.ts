import { APP_INITIALIZER, FactoryProvider } from '@angular/core';
import { AppConfiguration } from './app-configuration.model';
import { AppInitService } from './app-init.service';

function initAppConfigurationFactory(appLoadService: AppInitService): () => Promise<AppConfiguration> {
    return () => appLoadService.init();
}

export const APP_CONFIGURATION_PROVIDER: FactoryProvider = {
    provide: APP_INITIALIZER,
    useFactory: initAppConfigurationFactory,
    deps: [AppInitService],
    multi: true
};
