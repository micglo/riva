import { ChangeDetectorRef, Renderer2 } from '@angular/core';
import { async, ComponentFixture, inject, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule } from '@ngx-translate/core';
import {
    AuthService,
    AuthServiceStub,
    StorageModule,
    StorageService,
    TranslationLanguage,
    TranslationModule,
    UserService,
    UserServiceStub,
    WINDOW
} from 'riva-core';
import { SIDEBAR_BG_1_URL } from './../../constants/images.const';
import { LIGHT_DARK_COLORS } from './../../constants/light-dark-colors.const';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { BodyClassesUpdateService } from './../../services/body-classes-update.service';
import { CustomizerService } from './../../services/customizer.service';
import { FullLayoutService } from './../../services/full-layout.service';
import { LayoutService } from './../../services/layout.service';
import { LayoutTemplateStore } from './../../stores/layout-template.store';
import { ShowSmallScreenMenuStore } from './../../stores/show-small-screen-manu.store';
import { WINDOW_MOCK } from './../../testing/mocks/window.mock';
import { Renderer2Stub } from './../../testing/stubs/renderer2.stub';
import { CustomizerComponent } from './../customizer/customizer.component';
import { FooterComponent } from './../footer/footer.component';
import { HorizontalMenuComponent } from './../horizontal-menu/horizontal-menu.component';
import { NavbarFullscreenComponent } from './../navbar-fullscreen/navbar-fullscreen.component';
import { NavbarLanguageComponent } from './../navbar-language/navbar-language.component';
import { NavbarLogoComponent } from './../navbar-logo/navbar-logo.component';
import { NavbarNotificationsComponent } from './../navbar-notifications/navbar-notifications.component';
import { NavbarUserComponent } from './../navbar-user/navbar-user.component';
import { NavbarComponent } from './../navbar/navbar.component';
import { FullLayoutComponent } from './full-layout.component';

describe('FullLayoutComponent', () => {
    let component: FullLayoutComponent;
    let fixture: ComponentFixture<FullLayoutComponent>;
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

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [RouterTestingModule, TranslateModule.forRoot(), TranslationModule.forRoot(), StorageModule.forRoot()],
            declarations: [
                FullLayoutComponent,
                NavbarFullscreenComponent,
                NavbarLanguageComponent,
                NavbarLogoComponent,
                NavbarNotificationsComponent,
                NavbarUserComponent,
                NavbarComponent,
                FooterComponent,
                CustomizerComponent,
                HorizontalMenuComponent
            ],
            providers: [
                LayoutTemplateStore,
                ShowSmallScreenMenuStore,
                LayoutService,
                FullLayoutService,
                BodyClassesUpdateService,
                ChangeDetectorRef,
                { provide: WINDOW, useFactory: () => WINDOW_MOCK },
                CustomizerService,
                { provide: Renderer2, useClass: Renderer2Stub },
                { provide: AuthService, useClass: AuthServiceStub },
                { provide: UserService, useClass: UserServiceStub }
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(FullLayoutComponent);
        const storageService = TestBed.inject(StorageService);
        spyOn(storageService, 'getItem').and.returnValue(TranslationLanguage.English);
        component = fixture.componentInstance;
        component.layoutTemplate = layoutTemplate;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('ngOnInit should set properties when', () => {
        it('layout sidebar background image is true and variant is not transparent', inject(
            [LayoutTemplateStore, ShowSmallScreenMenuStore],
            (layoutTemplateStore: LayoutTemplateStore, showSmallScreenMenuStore: ShowSmallScreenMenuStore) => {
                const expectedIsSmallScreen = false;
                const expectedIsSmallScreenMenuShown = false;
                const expectedBgImage = layoutTemplate.sidebar.backgroundImageURL;
                const expectedBgColor = layoutTemplate.sidebar.backgroundColor;
                spyOn(layoutTemplateStore, 'updateState').and.callThrough();
                spyOn(showSmallScreenMenuStore, 'updateState').and.callThrough();
                layoutTemplate.sidebar.backgroundImage = true;
                layoutTemplate.variant = LayoutVariant.Light;
                component.layoutTemplate = layoutTemplate;

                component.ngOnInit();

                expect(component.isSmallScreen).toEqual(expectedIsSmallScreen);
                expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(layoutTemplate);
                expect(showSmallScreenMenuStore.updateState).toHaveBeenCalledWith(expectedIsSmallScreenMenuShown);
                expect(component.layoutTemplate).toEqual(layoutTemplate);
                expect(component.isSmallScreenMenuShown).toEqual(expectedIsSmallScreenMenuShown);
                expect(component.bgImage).toEqual(expectedBgImage);
                expect(component.bgColor).toEqual(expectedBgColor);
            }
        ));

        it('layout sidebar background image is false', inject(
            [LayoutTemplateStore, ShowSmallScreenMenuStore],
            (layoutTemplateStore: LayoutTemplateStore, showSmallScreenMenuStore: ShowSmallScreenMenuStore) => {
                const expectedIsSmallScreen = false;
                const expectedIsSmallScreenMenuShown = false;
                const expectedBgImage = '';
                const expectedBgColor = layoutTemplate.sidebar.backgroundColor;
                spyOn(layoutTemplateStore, 'updateState').and.callThrough();
                spyOn(showSmallScreenMenuStore, 'updateState').and.callThrough();
                layoutTemplate.variant = LayoutVariant.Light;
                layoutTemplate.sidebar.backgroundImage = false;
                component.layoutTemplate = layoutTemplate;

                component.ngOnInit();

                expect(component.isSmallScreen).toEqual(expectedIsSmallScreen);
                expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(layoutTemplate);
                expect(showSmallScreenMenuStore.updateState).toHaveBeenCalledWith(expectedIsSmallScreenMenuShown);
                expect(component.layoutTemplate).toEqual(layoutTemplate);
                expect(component.isSmallScreenMenuShown).toEqual(expectedIsSmallScreenMenuShown);
                expect(component.bgImage).toEqual(expectedBgImage);
                expect(component.bgColor).toEqual(expectedBgColor);
            }
        ));

        it('layout variant is transparent', inject(
            [LayoutTemplateStore, ShowSmallScreenMenuStore],
            (layoutTemplateStore: LayoutTemplateStore, showSmallScreenMenuStore: ShowSmallScreenMenuStore) => {
                const expectedIsSmallScreen = false;
                const expectedIsSmallScreenMenuShown = false;
                const expectedBgImage = '';
                const blackColorIndex = 7;
                const expectedBgColor = LIGHT_DARK_COLORS[blackColorIndex].backgroundColor;
                spyOn(layoutTemplateStore, 'updateState').and.callThrough();
                spyOn(showSmallScreenMenuStore, 'updateState').and.callThrough();
                layoutTemplate.variant = LayoutVariant.Transparent;
                layoutTemplate.sidebar.backgroundImage = true;
                component.layoutTemplate = layoutTemplate;

                component.ngOnInit();

                expect(component.isSmallScreen).toEqual(expectedIsSmallScreen);
                expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(layoutTemplate);
                expect(showSmallScreenMenuStore.updateState).toHaveBeenCalledWith(expectedIsSmallScreenMenuShown);
                expect(component.layoutTemplate).toEqual(layoutTemplate);
                expect(component.isSmallScreenMenuShown).toEqual(expectedIsSmallScreenMenuShown);
                expect(component.bgImage).toEqual(expectedBgImage);
                expect(component.bgColor).toEqual(expectedBgColor);
            }
        ));
    });

    it('scrollToTop should call window.scroll', inject([WINDOW], (window: Window) => {
        spyOn(window, 'scroll');
        component.scrollToTop();

        expect(window.scroll).toHaveBeenCalled();
    }));

    it('onOutsideClick should call updateState when element classes do not contain toggleSidebarNavbarButton', inject(
        [ShowSmallScreenMenuStore],
        (showSmallScreenMenuStore: ShowSmallScreenMenuStore) => {
            const dummyElement = document.createElement('div') as Element;
            const event = {
                currentTarget: dummyElement as EventTarget
            } as MouseEvent;
            spyOn(showSmallScreenMenuStore, 'updateState').and.callThrough();

            component.onOutsideClick(event);

            expect(showSmallScreenMenuStore.updateState).toHaveBeenCalledWith(false);
        }
    ));

    it('onResize should change isSmallScreen property', inject(
        [ShowSmallScreenMenuStore],
        (showSmallScreenMenuStore: ShowSmallScreenMenuStore) => {
            const event = ({
                currentTarget: window as EventTarget
            } as unknown) as UIEvent;
            spyOn(showSmallScreenMenuStore, 'updateState').and.callThrough();

            component.onResize(event);

            expect(component.isSmallScreen).toEqual(true);
        }
    ));
});
