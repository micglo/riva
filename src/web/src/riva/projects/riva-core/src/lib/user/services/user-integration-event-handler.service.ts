import { Injectable, OnDestroy } from '@angular/core';
import { combineLatest, Subscription } from 'rxjs';
import { AuthService } from './../../auth/services/auth.service';
import { NotificationService } from './../../notification/notification.service';
import { IntegrationEventFailure } from './../../signal-r/models/integration-event-failure.model';
import { IntegrationEvent } from './../../signal-r/models/integration-event.model';
import { IntegrationEventService } from './../../signal-r/services/integration-event.service';
import { IntegrationEventStore } from './../../signal-r/stores/integration-event.store';
import { UserDeleteCorrelationIdStore } from './../stores/user-delete-correlation-id.store';
import { UserUpdateCorrelationIdStore } from './../stores/user-update-correlation-id.store';

@Injectable()
export class UserUpdateIntegrationEventHandlerService implements OnDestroy {
    private _subscription = new Subscription();

    constructor(
        private integrationEventStore: IntegrationEventStore,
        private integrationEventService: IntegrationEventService,
        private userUpdateCorrelationIdStore: UserUpdateCorrelationIdStore,
        private userDeleteCorrelationIdStore: UserDeleteCorrelationIdStore,
        private notificationService: NotificationService,
        private authService: AuthService
    ) {}

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    public handleUserUpdateEvents(): void {
        const subscription = combineLatest([this.integrationEventStore.state$, this.userUpdateCorrelationIdStore.state$]).subscribe(
            (data: [(IntegrationEvent | IntegrationEventFailure)[], string[]]) => {
                data[0].forEach((integrationEvent: IntegrationEvent | IntegrationEventFailure) => {
                    data[1].forEach((correlationId: string) => {
                        if (integrationEvent.correlationId === correlationId) {
                            if (this.integrationEventService.isIntegrationEventFailure(integrationEvent)) {
                                this.notificationService.error('application.integrationEvent.userUpdated.failure');
                            } else {
                                this.notificationService.success('application.integrationEvent.userUpdated.success');
                            }

                            this.userUpdateCorrelationIdStore.removeCorrelationId(correlationId);
                            this.integrationEventStore.removeIntegrationEvent(integrationEvent);
                            return;
                        }
                    });
                });
            }
        );

        this._subscription.add(subscription);
    }

    public handleUserDeleteEvents(): void {
        const subscription = combineLatest([this.integrationEventStore.state$, this.userDeleteCorrelationIdStore.state$]).subscribe(
            (data: [(IntegrationEvent | IntegrationEventFailure)[], string[]]) => {
                data[0].forEach((integrationEvent: IntegrationEvent | IntegrationEventFailure) => {
                    data[1].forEach((correlationId: string) => {
                        if (integrationEvent.correlationId === correlationId) {
                            if (this.integrationEventService.isIntegrationEventFailure(integrationEvent)) {
                                this.notificationService.error('application.integrationEvent.userDeleted.failure');
                            } else {
                                this.notificationService.success('application.integrationEvent.userDeleted.success');
                            }

                            this.userDeleteCorrelationIdStore.removeCorrelationId(correlationId);
                            this.integrationEventStore.removeIntegrationEvent(integrationEvent);
                            const timeout = 2000;
                            setTimeout(() => this.authService.logOut(), timeout);
                            return;
                        }
                    });
                });
            }
        );

        this._subscription.add(subscription);
    }
}
