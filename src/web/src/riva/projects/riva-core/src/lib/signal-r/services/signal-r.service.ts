import { Injectable, NgZone, OnDestroy } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from '@aspnet/signalr';
import { Observable, Subscription } from 'rxjs';
import { AuthService } from './../../auth/services/auth.service';
import { AppConfigurationService } from './../../configuration/app-configuration.service';
import { SignalRMethod } from './../enums/signal-r-method.enum';
import { IntegrationEventFailure } from './../models/integration-event-failure.model';
import { IntegrationEvent } from './../models/integration-event.model';
import { IntegrationEventStore } from './../stores/integration-event.store';

@Injectable()
export class SignalRService implements OnDestroy {
    private _hubUrl: string;
    private _hubConnection: HubConnection;
    private _subscription = new Subscription();

    public get integrationEvents$(): Observable<Array<IntegrationEvent | IntegrationEventFailure>> {
        return this.integrationEventStore.state$;
    }

    constructor(
        appConfigurationService: AppConfigurationService,
        private ngZone: NgZone,
        private authService: AuthService,
        private integrationEventStore: IntegrationEventStore
    ) {
        this._hubUrl = `${appConfigurationService.configuration.api_url}/rivaHub`;
        Object.defineProperty(WebSocket, 'OPEN', { value: 1 });
    }

    ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    public initialize(isAuth: boolean): void {
        const options = isAuth
            ? this.createIHttpConnectionOptionsForAuthenticatedUser()
            : this.createIHttpConnectionOptionsForAnonymousUser();
        this.start(this._hubConnection, options);
    }

    private createIHttpConnectionOptionsForAuthenticatedUser(): IHttpConnectionOptions {
        return {
            transport: HttpTransportType.WebSockets,
            skipNegotiation: true,
            accessTokenFactory: () => this.resolveAccessTokenFactory()
        };
    }

    private createIHttpConnectionOptionsForAnonymousUser(): IHttpConnectionOptions {
        return {
            transport: HttpTransportType.WebSockets,
            skipNegotiation: true
        };
    }

    private start(hubConnection: HubConnection, options: IHttpConnectionOptions): void {
        if (hubConnection) {
            this.killHubConnection(hubConnection);
            this.createAndConfigureConnection(options);
        } else {
            const timeout = 5000;
            setTimeout(() => this.createAndConfigureConnection(options), timeout);
        }
    }

    private resolveAccessTokenFactory(): Promise<string> {
        return new Promise((resolve, reject) => {
            const subscription = this.authService.accessToken$.subscribe(resolve, reject);
            this._subscription.add(subscription);
        });
    }

    private killHubConnection(hubConnection: HubConnection): void {
        hubConnection.off(SignalRMethod.ReceivedIntegrationEvent);
        hubConnection.stop();
    }

    private createAndConfigureConnection(options: IHttpConnectionOptions): void {
        this._hubConnection = this.createHubConnection(options);
        this.registerServerEvents(this._hubConnection);
        this.startConnection(this._hubConnection, options);
    }

    private createHubConnection(options: IHttpConnectionOptions): HubConnection {
        return new HubConnectionBuilder().withUrl(this._hubUrl, options).build();
    }

    private registerServerEvents(hubConnection: HubConnection): void {
        hubConnection.on(SignalRMethod.ReceivedIntegrationEvent, (integrationEvent: IntegrationEvent | IntegrationEventFailure) => {
            this.ngZone.run(() => this.integrationEventStore.addIntegrationEvent(integrationEvent));
        });
    }

    private startConnection(hubConnection: HubConnection, options: IHttpConnectionOptions): void {
        hubConnection.start().catch(() => this.reconnect(hubConnection, options));
    }

    private reconnect(hubConnection: HubConnection, options: IHttpConnectionOptions): void {
        const reconnectTimeout = 15000;
        setTimeout(() => this.startConnection(hubConnection, options), reconnectTimeout);
    }
}
