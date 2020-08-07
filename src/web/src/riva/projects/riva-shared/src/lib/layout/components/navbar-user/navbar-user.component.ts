import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService, User, UserService } from 'riva-core';
import { Subscription } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { AVATAR_URL } from './../../constants/images.const';

@Component({
    selector: 'lib-navbar-user',
    templateUrl: './navbar-user.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavbarUserComponent implements OnInit, OnDestroy {
    private _subscription = new Subscription();
    private _avatarUrl = AVATAR_URL;

    public get avatarUrl(): string {
        return this._avatarUrl;
    }

    constructor(private authService: AuthService, private userService: UserService, private cdr: ChangeDetectorRef) {}

    public ngOnInit(): void {
        const userSubscription = this.userService.user$
            .pipe(
                filter((user: User) => user !== null),
                map((user: User) => user.picture)
            )
            .subscribe((picture: string) => {
                this._avatarUrl = picture ? picture : AVATAR_URL;
                this.cdr.markForCheck();
            });
        this._subscription.add(userSubscription);
    }

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    public logOut(): void {
        this.authService.logOut();
    }
}
