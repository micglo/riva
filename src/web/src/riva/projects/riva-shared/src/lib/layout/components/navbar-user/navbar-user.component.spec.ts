import { async, ComponentFixture, inject, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule } from '@ngx-translate/core';
import {
    AnnouncementSendingFrequency,
    AuthRole,
    AuthService,
    AuthServiceStub,
    FlatForRentAnnouncementPreference,
    RoomForRentAnnouncementPreference,
    UserService,
    UserServiceStub
} from 'riva-core';
import { of } from 'rxjs';
import { AVATAR_URL } from './../../constants/images.const';
import { NavbarUserComponent } from './navbar-user.component';

describe('NavbarUserComponent', () => {
    let component: NavbarUserComponent;
    let fixture: ComponentFixture<NavbarUserComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [RouterTestingModule, TranslateModule.forRoot()],
            declarations: [NavbarUserComponent],
            providers: [
                { provide: AuthService, useClass: AuthServiceStub },
                { provide: UserService, useClass: UserServiceStub }
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(NavbarUserComponent);
        component = fixture.componentInstance;

        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('ngOnInit should', () => {
        const authUser = {
            id: 'id',
            email: 'email@email.com',
            confirmed: true,
            roles: new Array<AuthRole>(AuthRole.User)
        };

        beforeEach(() => {
            const authService = TestBed.inject(AuthService);
            spyOnProperty(authService, 'authUser', 'get').and.returnValue(authUser);
        });

        it('load user', inject([UserService], (userService: UserService) => {
            spyOn(userService, 'loadUser').and.callThrough();

            component.ngOnInit();

            expect(userService.loadUser).toHaveBeenCalledWith(authUser.id);
        }));

        describe('set avatar url to', () => {
            const user = {
                id: 'id',
                email: 'email@email.com',
                picture: '',
                serviceActive: true,
                announcementPreferenceLimit: 2,
                announcementSendingFrequency: AnnouncementSendingFrequency.EveryFourHours,
                roomForRentAnnouncementPreferences: new Array<RoomForRentAnnouncementPreference>(),
                flatForRentAnnouncementPreferences: new Array<FlatForRentAnnouncementPreference>()
            };
            it('default url when user has no picture', inject([UserService], (userService: UserService) => {
                const picture = 'picture';
                user.picture = picture;
                spyOnProperty(userService, 'user$', 'get').and.returnValue(of(user));

                component.ngOnInit();

                expect(component.avatarUrl).toEqual(picture);
            }));

            it('user picture', inject([UserService], (userService: UserService) => {
                user.picture = '';
                spyOnProperty(userService, 'user$', 'get').and.returnValue(of(user));

                component.ngOnInit();

                expect(component.avatarUrl).toEqual(AVATAR_URL);
            }));
        });
    });

    it('logout should call authService.logout', inject([AuthService], (authService: AuthService) => {
        spyOn(authService, 'logOut').and.callThrough();

        component.logOut();

        expect(authService.logOut).toHaveBeenCalled();
    }));
});
