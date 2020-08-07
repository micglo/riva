import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { StorageService } from './storage.service';

@NgModule()
export class StorageModule {
    constructor(@Optional() @SkipSelf() parentModule: StorageModule) {
        if (parentModule) {
            throw new Error('StorageModule is already loaded. Import it in the AppModule only.');
        }
    }

    static forRoot(): ModuleWithProviders<StorageModule> {
        return {
            ngModule: StorageModule,
            providers: [StorageService]
        };
    }
}
