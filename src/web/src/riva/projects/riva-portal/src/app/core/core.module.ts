import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { IntegrationEventHandlerService } from './services/integration-event-handler.service';
import { AccountRegistrationCorrelationIdStore } from './stores/account-registration-correlation-id.store';

@NgModule()
export class CoreModule {
    constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
        if (parentModule) {
            throw new Error('CoreModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<CoreModule> {
        return {
            ngModule: CoreModule,
            providers: [AccountRegistrationCorrelationIdStore, IntegrationEventHandlerService]
        };
    }
}
