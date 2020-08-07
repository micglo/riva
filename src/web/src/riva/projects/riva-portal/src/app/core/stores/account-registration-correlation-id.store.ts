import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class AccountRegistrationCorrelationIdStore {
    private _subject$: BehaviorSubject<Array<string>>;

    public get state$(): Observable<Array<string>> {
        return this._subject$.asObservable();
    }

    constructor() {
        this._subject$ = new BehaviorSubject<Array<string>>(Array<string>());
    }

    public addCorrelationId(correlationId: string): void {
        this._subject$.next([...this._subject$.getValue(), correlationId]);
    }

    public removeCorrelationId(correlationId: string): void {
        const correlationIdsToUpdate = this._subject$.getValue().filter((ie: string) => ie !== correlationId);
        this._subject$.next(correlationIdsToUpdate);
    }
}
