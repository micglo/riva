import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class AuthStore {
    private _subject$: BehaviorSubject<boolean>;

    public get state$(): Observable<boolean> {
        return this._subject$.asObservable();
    }

    constructor() {
        this._subject$ = new BehaviorSubject<boolean>(false);
    }

    public updateState(state: boolean): void {
        this._subject$.next(state);
    }
}
