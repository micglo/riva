import { OAuthModuleConfig } from 'angular-oauth2-oidc';
import { AppConfigurationService } from './../../configuration';

export function oAuthModuleConfigFactory(appConfigurationService: AppConfigurationService): OAuthModuleConfig {
    return {
        resourceServer: {
            allowedUrls: [appConfigurationService.configuration.api_url],
            sendAccessToken: true
        }
    } as OAuthModuleConfig;
}
