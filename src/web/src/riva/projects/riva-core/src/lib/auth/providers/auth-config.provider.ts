import { FactoryProvider } from '@angular/core';
import { AuthConfig } from 'angular-oauth2-oidc';
import { AppConfigurationService } from './../../configuration/app-configuration.service';
import { authConfigFactory } from './../factories/auth-config.factory';

export const AUTH_CONFIG_PROVIDER: FactoryProvider = {
    provide: AuthConfig,
    useFactory: authConfigFactory,
    deps: [AppConfigurationService]
};
