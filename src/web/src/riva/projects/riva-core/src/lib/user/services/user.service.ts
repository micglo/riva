import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { filter, switchMap, tap } from 'rxjs/operators';
import { AuthService } from './../../auth/services/auth.service';
import { UpdateUser } from './../models/update-user.model';
import { User } from './../models/user.model';
import { UserProxy } from './../proxies/user.proxy';
import { UserUpdateCorrelationIdStore } from './../stores/user-update-correlation-id.store';
import { UserStore } from './../stores/user.store';

@Injectable()
export class UserService {
    public get user$(): Observable<User> {
        return this.userStore.state$;
    }

    constructor(
        private userProxy: UserProxy,
        private userStore: UserStore,
        private authService: AuthService,
        private userUpdateCorrelationIdStore: UserUpdateCorrelationIdStore
    ) {}

    public loadUser$(): Observable<User> {
        return this.authService.isAuthenticated$.pipe(
            filter((isAuth: boolean) => isAuth),
            switchMap(() => this.user$),
            filter((user: User) => !user),
            switchMap(() => this.userProxy.getById(this.authService.authUser.id)),
            tap((user: User) => this.userStore.updateState(user))
        );
    }

    update(updateUser: UpdateUser): Observable<User> {
        return this.userProxy.update(updateUser).pipe(
            tap((correlationId: string) => this.userUpdateCorrelationIdStore.addCorrelationId(correlationId)),
            switchMap(() => this.userProxy.getById(updateUser.id)),
            tap((user: User) => this.userStore.updateState(user))
        );
    }
}
