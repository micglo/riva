import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { LoggedInGuard } from './guards/logged-in.guard';
import { PreventLoggedInGuard } from './guards/prevent-logged-in.guard';
import { AUTH_CONFIG_PROVIDER } from './providers/auth-config.provider';
import { O_AUTH_MODULE_CONFIG_PROVIDER } from './providers/o-auth-module-config.provider';
import { AuthService } from './services/auth.service';
import { AccessTokenStore } from './stores/access-token.store';
import { AuthStore } from './stores/auth.store';

@NgModule()
export class AuthModule {
    constructor(@Optional() @SkipSelf() parentModule: AuthModule) {
        if (parentModule) {
            throw new Error('AuthModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<AuthModule> {
        return {
            ngModule: AuthModule,
            providers: [
                AuthStore,
                AccessTokenStore,
                AuthService,
                LoggedInGuard,
                PreventLoggedInGuard,
                AUTH_CONFIG_PROVIDER,
                O_AUTH_MODULE_CONFIG_PROVIDER
            ]
        };
    }
}
