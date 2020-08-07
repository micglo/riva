import { Observable, of } from 'rxjs';
import { AuthRole } from './../enums/auth-role.enum';
import { AuthUser } from './../models/auth-user.model';

export class AuthServiceStub {
    public get isAuthenticated$(): Observable<boolean> {
        return of(false);
    }

    public get authUser(): AuthUser {
        return {
            id: 'id',
            email: 'email@email.com',
            confirmed: true,
            roles: new Array<AuthRole>(AuthRole.User)
        };
    }

    public initialize$(): Observable<void> {
        return of();
    }

    public login(targetUrl: string): void {}

    public logOut(): void {}
}
