import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class AccessTokenStore {
    private _subject$: BehaviorSubject<string>;

    public get state$(): Observable<string> {
        return this._subject$.asObservable();
    }

    constructor() {
        this._subject$ = new BehaviorSubject<string>(null);
    }

    public updateState(state: string): void {
        this._subject$.next(state);
    }
}
