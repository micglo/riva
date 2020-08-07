import { Injectable, OnDestroy } from '@angular/core';
import {
    IntegrationEvent,
    IntegrationEventFailure,
    IntegrationEventService,
    IntegrationEventStore,
    NotificationService,
    UserUpdateIntegrationEventHandlerService
} from 'riva-core';
import { combineLatest, Subscription } from 'rxjs';
import { AccountRegistrationCorrelationIdStore } from './../stores/account-registration-correlation-id.store';

@Injectable()
export class IntegrationEventHandlerService implements OnDestroy {
    private _subscription = new Subscription();

    constructor(
        private integrationEventStore: IntegrationEventStore,
        private integrationEventService: IntegrationEventService,
        private accountRegistrationCorrelationIdStore: AccountRegistrationCorrelationIdStore,
        private notificationService: NotificationService,
        private userUpdateIntegrationEventHandlerService: UserUpdateIntegrationEventHandlerService
    ) {}

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    public initialize(): void {
        this.handleAccountRegistrationEvents();
        this.userUpdateIntegrationEventHandlerService.handleUserUpdateEvents();
        this.userUpdateIntegrationEventHandlerService.handleUserDeleteEvents();
    }

    private handleAccountRegistrationEvents(): void {
        const subscription = combineLatest([
            this.integrationEventStore.state$,
            this.accountRegistrationCorrelationIdStore.state$
        ]).subscribe((data: [(IntegrationEvent | IntegrationEventFailure)[], string[]]) => {
            data[0].forEach((integrationEvent: IntegrationEvent | IntegrationEventFailure) => {
                data[1].forEach((correlationId: string) => {
                    if (integrationEvent.correlationId === correlationId) {
                        if (this.integrationEventService.isIntegrationEventFailure(integrationEvent)) {
                            this.notificationService.error('application.integrationEvent.accountCreated.failure');
                        } else {
                            this.notificationService.success('application.integrationEvent.accountCreated.success');
                        }

                        this.accountRegistrationCorrelationIdStore.removeCorrelationId(correlationId);
                        this.integrationEventStore.removeIntegrationEvent(integrationEvent);
                        return;
                    }
                });
            });
        });

        this._subscription.add(subscription);
    }
}
