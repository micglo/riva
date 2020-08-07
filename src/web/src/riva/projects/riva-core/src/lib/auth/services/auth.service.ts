import { Injectable, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthEvent, OAuthService } from 'angular-oauth2-oidc';
import { combineLatest, from, Observable, Subscription } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';
import { AuthRole } from './../enums/auth-role.enum';
import { IdentityClaim } from './../enums/identity-claim.enum';
import { AuthUser } from './../models/auth-user.model';
import { AccessTokenStore } from './../stores/access-token.store';
import { AuthStore } from './../stores/auth.store';

@Injectable()
export class AuthService implements OnDestroy {
    private _subscription = new Subscription();

    public get isAuthenticated$(): Observable<boolean> {
        return this.authStore.state$;
    }

    public get accessToken$(): Observable<string> {
        return this.accessTokenStore.state$;
    }

    public get isAdmin(): boolean {
        return this.authUser.roles.includes(AuthRole.Administrator);
    }

    public get authUser(): AuthUser | null {
        const claims = this.oauthService.getIdentityClaims();
        if (!claims) {
            return null;
        }
        return {
            id: claims[IdentityClaim.Sub],
            email: claims[IdentityClaim.Email],
            confirmed: claims[IdentityClaim.EmailVerified],
            roles: String(claims[IdentityClaim.Role]).split(',') as Array<AuthRole>
        };
    }

    constructor(
        private oauthService: OAuthService,
        private authStore: AuthStore,
        private accessTokenStore: AccessTokenStore,
        private router: Router
    ) {
        this.updateIsAuthenticatedStateOnReceivedEvent();
        this.loadUserProfileOnTokenReceivedEvent();
        this.updateAccessTokenStateOnEvent();
        this.oauthService.setupAutomaticSilentRefresh();
    }

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    public initialize$(): Observable<void> {
        const initPromise = this.oauthService
            .loadDiscoveryDocument()
            .then(() => this.oauthService.tryLogin())
            .then(async () => {
                if (this.oauthService.hasValidAccessToken()) {
                    return Promise.resolve();
                }
                await this.refreshTokenSilently();
            })
            .then(() => {
                if (this.oauthService.state && this.oauthService.state !== 'undefined' && this.oauthService.state !== 'null') {
                    this.navigateToStateUrl();
                }
            });

        return from(initPromise);
    }

    public login(targetUrl: string): void {
        this.oauthService.initLoginFlow(targetUrl);
    }

    public logOut(): void {
        this.oauthService.logOut();
        this.router.navigate(['/']);
    }

    private updateIsAuthenticatedStateOnReceivedEvent(): void {
        const allEventsSubscrition = combineLatest([this.oauthService.events, this.authStore.state$])
            .pipe(filter((data: [OAuthEvent, boolean]) => data[1] !== this.oauthService.hasValidAccessToken()))
            .subscribe(() => this.authStore.updateState(this.oauthService.hasValidAccessToken()));
        this._subscription.add(allEventsSubscrition);
    }

    private loadUserProfileOnTokenReceivedEvent(): void {
        const userInfoSubscription = this.oauthService.events
            .pipe(
                filter((e: OAuthEvent) => e.type === 'token_received'),
                switchMap(() => from(this.oauthService.loadUserProfile()))
            )
            .subscribe();
        this._subscription.add(userInfoSubscription);
    }

    private updateAccessTokenStateOnEvent(): void {
        const updateAccessTokenStateSubscription = combineLatest([this.oauthService.events, this.accessTokenStore.state$])
            .pipe(
                filter(
                    (data: [OAuthEvent, string]) =>
                        this.oauthService.hasValidAccessToken() && data[1] !== this.oauthService.getAccessToken()
                )
            )
            .subscribe(() => this.accessTokenStore.updateState(this.oauthService.getAccessToken()));
        this._subscription.add(updateAccessTokenStateSubscription);
    }

    private async refreshTokenSilently(): Promise<void> {
        try {
            await this.oauthService.silentRefresh();
            return await Promise.resolve();
        } catch (result) {
            const errorResponsesRequiringUserInteraction = [
                'interaction_required',
                'login_required',
                'account_selection_required',
                'consent_required'
            ];
            return result && result.reason && errorResponsesRequiringUserInteraction.indexOf(result.reason.error) >= 0
                ? Promise.resolve()
                : Promise.reject(result);
        }
    }

    private navigateToStateUrl(): void {
        let stateUrl = this.oauthService.state;
        if (stateUrl.startsWith('/') === false) {
            stateUrl = decodeURIComponent(stateUrl);
        }
        this.router.navigateByUrl(stateUrl);
    }
}
