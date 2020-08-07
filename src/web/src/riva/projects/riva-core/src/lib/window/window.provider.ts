import { FactoryProvider, PLATFORM_ID } from '@angular/core';
import { WindowReferenceBase } from './window-reference-base.model';
import { windowFactory } from './window.factory';
import { WINDOW } from './window.injection-token';

export const WINDOW_PROVIDER: FactoryProvider = {
    provide: WINDOW,
    useFactory: windowFactory,
    deps: [WindowReferenceBase, PLATFORM_ID]
};
