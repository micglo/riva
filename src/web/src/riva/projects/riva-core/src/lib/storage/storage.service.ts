import { Inject, Injectable } from '@angular/core';
import { WINDOW } from './../window/window.injection-token';
import { JsonStorageTranscoder } from './storage-transcoder';

@Injectable()
export class StorageService {
    constructor(@Inject(WINDOW) private window: Window) {}

    public getItem<T>(key: string): T | undefined {
        const value = this.storage.getItem(key);
        return value != null ? JsonStorageTranscoder.decode(value) : undefined;
    }

    public setItem<T>(key: string, value: T): void {
        const stringValue = JsonStorageTranscoder.encode(value);
        return this.storage.setItem(key, stringValue);
    }

    private get storage(): Storage {
        return this.window.localStorage;
    }
}
