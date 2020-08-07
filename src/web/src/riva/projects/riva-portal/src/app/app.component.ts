import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService, SignalRService, TranslationService, UserService } from 'riva-core';
import { Subscription } from 'rxjs';
import { IntegrationEventHandlerService } from './core/services/integration-event-handler.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {
    private _subscription = new Subscription();

    constructor(
        private translationService: TranslationService,
        private authService: AuthService,
        private userService: UserService,
        private signalRService: SignalRService,
        private integrationEventHandlerService: IntegrationEventHandlerService
    ) {}

    public ngOnInit(): void {
        this.translationService.initialize();
        this.integrationEventHandlerService.initialize();
        const authInitSubscription = this.authService.initialize$().subscribe();
        const loadUserSubscription = this.userService.loadUser$().subscribe();
        const signalRInitSubscription = this.authService.isAuthenticated$.subscribe((isAuth: boolean) =>
            this.signalRService.initialize(isAuth)
        );
        this._subscription.add(authInitSubscription);
        this._subscription.add(loadUserSubscription);
        this._subscription.add(signalRInitSubscription);
    }

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }
}
