import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { UserProxy } from './proxies/user.proxy';
import { UserUpdateIntegrationEventHandlerService } from './services/user-integration-event-handler.service';
import { UserService } from './services/user.service';
import { UserDeleteCorrelationIdStore } from './stores/user-delete-correlation-id.store';
import { UserUpdateCorrelationIdStore } from './stores/user-update-correlation-id.store';
import { UserStore } from './stores/user.store';

@NgModule()
export class UserModule {
    constructor(@Optional() @SkipSelf() parentModule: UserModule) {
        if (parentModule) {
            throw new Error('UserModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<UserModule> {
        return {
            ngModule: UserModule,
            providers: [
                UserStore,
                UserDeleteCorrelationIdStore,
                UserUpdateCorrelationIdStore,
                UserProxy,
                UserService,
                UserUpdateIntegrationEventHandlerService
            ]
        };
    }
}
