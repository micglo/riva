import { FactoryProvider } from '@angular/core';
import { OAuthModuleConfig } from 'angular-oauth2-oidc';
import { AppConfigurationService } from './../../configuration/app-configuration.service';
import { oAuthModuleConfigFactory } from './../factories/o-auth-module-config.factory';

export const O_AUTH_MODULE_CONFIG_PROVIDER: FactoryProvider = {
    provide: OAuthModuleConfig,
    useFactory: oAuthModuleConfigFactory,
    deps: [AppConfigurationService]
};
