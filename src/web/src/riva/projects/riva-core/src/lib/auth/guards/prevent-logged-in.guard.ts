import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    CanActivate,
    CanActivateChild,
    CanLoad,
    Route,
    Router,
    RouterStateSnapshot,
    UrlSegment
} from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './../services/auth.service';

@Injectable()
export class PreventLoggedInGuard implements CanLoad, CanActivate, CanActivateChild {
    constructor(private authService: AuthService, private router: Router) {}

    public canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> {
        return this.isNotAuthenticated$(this.router.url);
    }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.isNotAuthenticated$(this.router.url);
    }

    public canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.isNotAuthenticated$(this.router.url);
    }

    private isNotAuthenticated$(url: string): Observable<boolean> {
        return this.authService.isAuthenticated$.pipe(
            map((isAuthenticated: boolean) => {
                if (isAuthenticated) {
                    this.router.navigateByUrl(url);
                    return false;
                }
                return true;
            })
        );
    }
}
