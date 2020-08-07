import { ChangeDetectorRef } from '@angular/core';
import { async, ComponentFixture, inject, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService, AuthServiceStub, StorageModule, TranslationModule, UserService, UserServiceStub, WindowModule } from 'riva-core';
import { of } from 'rxjs';
import { SIDEBAR_BG_1_URL } from './../../constants/images.const';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { LayoutTemplateStore } from './../../stores/layout-template.store';
import { ShowSmallScreenMenuStore } from './../../stores/show-small-screen-manu.store';
import { NavbarFullscreenComponent } from './../navbar-fullscreen/navbar-fullscreen.component';
import { NavbarLanguageComponent } from './../navbar-language/navbar-language.component';
import { NavbarLogoComponent } from './../navbar-logo/navbar-logo.component';
import { NavbarNotificationsComponent } from './../navbar-notifications/navbar-notifications.component';
import { NavbarUserComponent } from './../navbar-user/navbar-user.component';
import { NavbarComponent } from './navbar.component';

describe('NavbarComponent', () => {
    let component: NavbarComponent;
    let fixture: ComponentFixture<NavbarComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                RouterTestingModule,
                TranslateModule.forRoot(),
                TranslationModule.forRoot(),
                StorageModule.forRoot(),
                WindowModule.forRoot()
            ],
            declarations: [
                NavbarComponent,
                NavbarFullscreenComponent,
                NavbarLanguageComponent,
                NavbarLogoComponent,
                NavbarNotificationsComponent,
                NavbarUserComponent
            ],
            providers: [
                ChangeDetectorRef,
                LayoutTemplateStore,
                ShowSmallScreenMenuStore,
                { provide: AuthService, useClass: AuthServiceStub },
                { provide: UserService, useClass: UserServiceStub }
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(NavbarComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('ngOnInit should set properties', inject(
        [LayoutTemplateStore, ShowSmallScreenMenuStore],
        (layoutTemplateStore: LayoutTemplateStore, showSmallScreenMenuStore: ShowSmallScreenMenuStore) => {
            const layoutTemplate: LayoutTemplate = {
                variant: LayoutVariant.Light,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };
            const expectedBackgroundColor = '';

            spyOnProperty(layoutTemplateStore, 'state$', 'get').and.returnValue(of(layoutTemplate));
            spyOnProperty(showSmallScreenMenuStore, 'state$', 'get').and.returnValue(of(false));

            component.ngOnInit();

            expect(component.backgroundColor).toEqual(expectedBackgroundColor);
            expect(component.isSmallScreen).toEqual(true);
            expect(component.layoutTemplate).toEqual(layoutTemplate);
        }
    ));

    it('toggleSidebar should call showSmallScreenMenuStore.updateState with opposite value', inject(
        [LayoutTemplateStore, ShowSmallScreenMenuStore],
        (layoutTemplateStore: LayoutTemplateStore, showSmallScreenMenuStore: ShowSmallScreenMenuStore) => {
            const layoutTemplate: LayoutTemplate = {
                variant: LayoutVariant.Light,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };
            const isSmallScreenMenuShown = false;

            spyOnProperty(layoutTemplateStore, 'state$', 'get').and.returnValue(of(layoutTemplate));
            spyOnProperty(showSmallScreenMenuStore, 'state$', 'get').and.returnValue(of(isSmallScreenMenuShown));
            spyOn(showSmallScreenMenuStore, 'updateState').and.callThrough();
            component.ngOnInit();

            component.toggleSidebar();

            expect(showSmallScreenMenuStore.updateState).toHaveBeenCalledWith(!isSmallScreenMenuShown);
        }
    ));

    it('onResize should change isSmallScreen property', () => {
        const event = ({
            currentTarget: window as EventTarget
        } as unknown) as UIEvent;

        component.onResize(event);

        expect(component.isSmallScreen).toEqual(true);
    });

    it('login should call authService.login', inject([AuthService, Router], (authService: AuthService, router: Router) => {
        const url = 'http://localhost:4200';
        spyOn(authService, 'login').and.callThrough();
        spyOnProperty(router, 'url', 'get').and.returnValue(url);

        component.login();

        expect(authService.login).toHaveBeenCalledWith(url);
    }));
});
