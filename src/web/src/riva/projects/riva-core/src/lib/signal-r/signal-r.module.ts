import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { IntegrationEventService } from './services/integration-event.service';
import { SignalRService } from './services/signal-r.service';
import { IntegrationEventStore } from './stores/integration-event.store';

@NgModule()
export class SignalRModule {
    constructor(@Optional() @SkipSelf() parentModule: SignalRModule) {
        if (parentModule) {
            throw new Error('SignalRModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<SignalRModule> {
        return {
            ngModule: SignalRModule,
            providers: [IntegrationEventStore, IntegrationEventService, SignalRService]
        };
    }
}
