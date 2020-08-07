import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { MyAccountServicesModule } from './../../my-account-services.module';
import { MyAccount } from './../models/my-account.model';

@Injectable({ providedIn: MyAccountServicesModule })
export class MyAccountStore {
    private _subject$: BehaviorSubject<MyAccount>;

    public get state$(): Observable<MyAccount> {
        return this._subject$.asObservable();
    }

    constructor() {
        this._subject$ = new BehaviorSubject<MyAccount>(null);
    }

    public updateState(myAccount: MyAccount): void {
        this._subject$.next(myAccount);
    }
}
