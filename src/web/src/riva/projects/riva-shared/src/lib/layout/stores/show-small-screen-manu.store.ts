import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LayoutServicesModule } from './../layout-services.module';

@Injectable({ providedIn: LayoutServicesModule })
export class ShowSmallScreenMenuStore {
    private _subject$: BehaviorSubject<boolean>;

    public get state$(): Observable<boolean> {
        return this._subject$.asObservable();
    }

    constructor() {
        this._subject$ = new BehaviorSubject<boolean>(false);
    }

    public updateState(isOpen: boolean): void {
        this._subject$.next(isOpen);
    }
}
