import { Injectable } from '@angular/core';
import { IntegrationEventFailure } from './../models/integration-event-failure.model';
import { IntegrationEvent } from './../models/integration-event.model';

@Injectable()
export class IntegrationEventService {
    public isIntegrationEventFailure(
        integrationEvent: IntegrationEvent | IntegrationEventFailure
    ): integrationEvent is IntegrationEventFailure {
        return 'reason' in integrationEvent;
    }
}
