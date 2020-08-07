import { isPlatformBrowser } from '@angular/common';
import { BrowserWindowReference } from './browser-window-reference.model';

// tslint:disable-next-line: ban-types
export function windowFactory(browserWindowReference: BrowserWindowReference, platformId: Object): Window | Object {
    if (isPlatformBrowser(platformId)) {
        return browserWindowReference.nativeWindow;
    }
    return new Object();
}
