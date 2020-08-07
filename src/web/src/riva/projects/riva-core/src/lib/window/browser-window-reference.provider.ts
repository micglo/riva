import { ClassProvider } from '@angular/core';
import { BrowserWindowReference } from './browser-window-reference.model';
import { WindowReferenceBase } from './window-reference-base.model';

export const BROWSER_WINDOW_REFERENCE_PROVIDER: ClassProvider = {
    provide: WindowReferenceBase,
    useClass: BrowserWindowReference
};
