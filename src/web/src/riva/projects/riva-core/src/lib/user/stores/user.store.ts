import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from './../models/user.model';

@Injectable()
export class UserStore {
    private _subject$: BehaviorSubject<User>;

    public get state$(): Observable<User> {
        return this._subject$.asObservable();
    }

    constructor() {
        this._subject$ = new BehaviorSubject<User>(null);
    }

    public updateState(user: User): void {
        this._subject$.next(user);
    }
}
