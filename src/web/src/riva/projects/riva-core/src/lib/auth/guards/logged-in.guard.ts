import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, CanLoad, Route, RouterStateSnapshot, UrlSegment } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthService } from './../services/auth.service';

@Injectable()
export class LoggedInGuard implements CanLoad, CanActivate, CanActivateChild {
    constructor(private authService: AuthService) {}

    public canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> {
        return this.isAuthenticated$(route.path);
    }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.isAuthenticated$(state.url);
    }

    public canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.isAuthenticated$(state.url);
    }

    private isAuthenticated$(url: string): Observable<boolean> {
        return this.authService.isAuthenticated$.pipe(tap((isAuthenticated: boolean) => isAuthenticated || this.authService.login(url)));
    }
}
