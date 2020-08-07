import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { NotificationService } from './notification.service';

@NgModule()
export class NotificationModule {
    constructor(@Optional() @SkipSelf() parentModule: NotificationModule) {
        if (parentModule) {
            throw new Error('NotificationModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<NotificationModule> {
        return {
            ngModule: NotificationModule,
            providers: [NotificationService]
        };
    }
}
