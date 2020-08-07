import { AuthConfig } from 'angular-oauth2-oidc';
import { AppConfigurationService } from './../../configuration/app-configuration.service';

export function authConfigFactory(appConfigurationService: AppConfigurationService): AuthConfig {
    return {
        issuer: appConfigurationService.configuration.auth.issuer,
        clientId: appConfigurationService.configuration.auth.client_id,
        responseType: 'code',
        redirectUri: appConfigurationService.configuration.auth.redirect_uri,
        silentRefreshRedirectUri: appConfigurationService.configuration.auth.silent_refresh_redirect_uri,
        postLogoutRedirectUri: appConfigurationService.configuration.auth.post_logout_redirect_uri,
        scope: appConfigurationService.configuration.auth.scope,
        useSilentRefresh: true,
        timeoutFactor: 0.75,
        sessionChecksEnabled: true,
        clearHashAfterLogin: false,
        nonceStateSeparator: 'semicolon',
        showDebugInformation: false
    } as AuthConfig;
}
