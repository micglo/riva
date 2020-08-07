import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { IntegrationEventFailure } from './../models/integration-event-failure.model';
import { IntegrationEvent } from './../models/integration-event.model';

@Injectable()
export class IntegrationEventStore {
    private _subject$: BehaviorSubject<Array<IntegrationEvent | IntegrationEventFailure>>;

    public get state$(): Observable<Array<IntegrationEvent | IntegrationEventFailure>> {
        return this._subject$.asObservable();
    }

    constructor() {
        this._subject$ = new BehaviorSubject<Array<IntegrationEvent | IntegrationEventFailure>>(
            Array<IntegrationEvent | IntegrationEventFailure>()
        );
    }

    public addIntegrationEvent(integrationEvent: IntegrationEvent | IntegrationEventFailure): void {
        this._subject$.next([...this._subject$.getValue(), integrationEvent]);
    }

    public removeIntegrationEvent(integrationEvent: IntegrationEvent | IntegrationEventFailure): void {
        const integrationEventsToUpdate = this._subject$
            .getValue()
            .filter((ie: IntegrationEvent | IntegrationEventFailure) => ie !== integrationEvent);
        this._subject$.next(integrationEventsToUpdate);
    }
}
