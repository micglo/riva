import { WindowReferenceBase } from './window-reference-base.model';

export class BrowserWindowReference extends WindowReferenceBase {
    constructor() {
        super();
    }

    get nativeWindow(): Window {
        return window;
    }
}
