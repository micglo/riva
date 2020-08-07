import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService, SpinnerService } from 'riva-core';
import { Subscription } from 'rxjs';
import { filter, switchMap, tap } from 'rxjs/operators';
import { Tab } from './../../common/enums/tab.enum';
import { MyAccount } from './../../common/models/my-account.model';
import { AccountService } from './../../common/services/account.service';
import { MyAccountStore } from './../../common/stores/my-account.store';

@Component({
    selector: 'lib-profile',
    templateUrl: './profile.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProfileComponent implements OnInit, OnDestroy {
    public tab = Tab;

    private _myAccount: MyAccount;
    private _subscription = new Subscription();
    private _activeTab = Tab.General;

    public get myAccount(): MyAccount {
        return this._myAccount;
    }

    public get activeTab(): Tab {
        return this._activeTab;
    }
    public set activeTab(value: Tab) {
        this._activeTab = value;
    }

    constructor(
        private authService: AuthService,
        private accountService: AccountService,
        private myAccountStore: MyAccountStore,
        private spinnerService: SpinnerService,
        private cdr: ChangeDetectorRef
    ) {}

    public ngOnInit(): void {
        this.spinnerService.show();
        this.loadMyAccount();
    }

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    public setActiveTab(tab: Tab): void {
        this._activeTab = tab;
    }

    public onPasswordAssigned(): void {
        this._activeTab = Tab.General;
    }

    private loadMyAccount(): void {
        const loadMyAccountSubscription = this.accountService
            .getById(this.authService.authUser.id)
            .pipe(
                tap((myAccount: MyAccount) => this.myAccountStore.updateState(myAccount)),
                switchMap(() => this.myAccountStore.state$.pipe(filter((myAccount: MyAccount) => myAccount !== null)))
            )
            .subscribe((myAccount: MyAccount) => {
                this._myAccount = myAccount;
                this.spinnerService.hide();
                this.cdr.markForCheck();
            });
        this._subscription.add(loadMyAccountSubscription);
    }
}
