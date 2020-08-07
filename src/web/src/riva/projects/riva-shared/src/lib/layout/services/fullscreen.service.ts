import { Injectable } from '@angular/core';
import * as screenfull from 'screenfull';
import { LayoutServicesModule } from './../layout-services.module';

@Injectable({ providedIn: LayoutServicesModule })
export class FullscreenService {
    private _fullscreenLib;

    constructor() {
        this._fullscreenLib = screenfull;
    }

    public toggle(): void {
        if (this._fullscreenLib.isEnabled) {
            this._fullscreenLib.toggle();
        }
    }
}
