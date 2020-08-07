import { HttpClientTestingModule } from '@angular/common/http/testing';
import { inject, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule, OAuthService } from 'angular-oauth2-oidc';
import { AuthRole } from './../enums/auth-role.enum';
import { IdentityClaim } from './../enums/identity-claim.enum';
import { AuthStore } from './../stores/auth.store';
import { AuthService } from './auth.service';

describe('AuthService', () => {
    let service: AuthService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
            providers: [AuthStore, AuthService]
        });
        service = TestBed.inject(AuthService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('authUser should return', () => {
        it('AuthUser instance', inject([OAuthService], (oAuthService: OAuthService) => {
            const claims = {
                sub: 'sub',
                email: 'email@email.com',
                email_verified: true,
                role: 'User'
            };
            const expectedResult = {
                id: claims[IdentityClaim.Sub],
                email: claims[IdentityClaim.Email],
                confirmed: claims[IdentityClaim.EmailVerified],
                roles: new Array<AuthRole>(AuthRole.User)
            };
            spyOn(oAuthService, 'getIdentityClaims').and.returnValue(claims);

            expect(service.authUser).toEqual(expectedResult);
        }));
        it('null', inject([OAuthService], (oAuthService: OAuthService) => {
            const expectedResult = null;
            spyOn(oAuthService, 'getIdentityClaims').and.returnValue(null);

            expect(service.authUser).toEqual(expectedResult);
        }));
    });

    it('login should call oAuthService.initLoginFlow with url argument', inject([OAuthService], (oAuthService: OAuthService) => {
        const url = 'url';
        spyOn(oAuthService, 'initLoginFlow').and.callThrough();

        service.login(url);

        expect(oAuthService.initLoginFlow).toHaveBeenCalledWith(url);
    }));

    it('logOut should call oAuthService.logOut', inject([OAuthService], (oAuthService: OAuthService) => {
        spyOn(oAuthService, 'logOut').and.callThrough();

        service.logOut();

        expect(oAuthService.logOut).toHaveBeenCalled();
    }));
});
