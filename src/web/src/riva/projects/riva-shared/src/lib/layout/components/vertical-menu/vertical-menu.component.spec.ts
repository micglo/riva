import { ChangeDetectorRef } from '@angular/core';
import { async, ComponentFixture, inject, TestBed } from '@angular/core/testing';
import { DeviceDetectorService } from 'ngx-device-detector';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { of } from 'rxjs';
import { DARK_LOGO_URL, SIDEBAR_BG_1_URL } from './../../constants/images.const';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { LayoutTemplateStore } from './../../stores/layout-template.store';
import { ShowSmallScreenMenuStore } from './../../stores/show-small-screen-manu.store';
import { VerticalMenuComponent } from './vertical-menu.component';

describe('VerticalMenuComponent', () => {
    let component: VerticalMenuComponent;
    let fixture: ComponentFixture<VerticalMenuComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [PerfectScrollbarModule],
            declarations: [VerticalMenuComponent],
            providers: [LayoutTemplateStore, ShowSmallScreenMenuStore, DeviceDetectorService, ChangeDetectorRef]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(VerticalMenuComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('ngOnInit should set properties', inject(
        [LayoutTemplateStore, DeviceDetectorService],
        (layoutTemplateStore: LayoutTemplateStore, deviceDetectorService: DeviceDetectorService) => {
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
            spyOnProperty(layoutTemplateStore, 'state$', 'get').and.returnValue(of(layoutTemplate));
            spyOn(deviceDetectorService, 'isMobile').and.returnValue(false);

            component.ngOnInit();

            expect(component.perfectScrollbarEnabled).toEqual(false);
            expect(component.layoutTemplate).toEqual(layoutTemplate);
            expect(component.logoUrl).toEqual(DARK_LOGO_URL);
        }
    ));

    it('toggleSidebar should update layout template state with opposite isCollapsed value', inject(
        [LayoutTemplateStore],
        (layoutTemplateStore: LayoutTemplateStore) => {
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
            const expectedLayoutTemplate: LayoutTemplate = {
                variant: LayoutVariant.Light,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: true,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };
            spyOnProperty(layoutTemplateStore, 'state$', 'get').and.returnValue(of(layoutTemplate));
            spyOn(layoutTemplateStore, 'updateState').and.callThrough();
            component.ngOnInit();

            component.toggleSidebar();

            expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
        }
    ));

    it('closeSidebar should call showSmallScreenMenuStore.updateState with false', inject(
        [ShowSmallScreenMenuStore],
        (showSmallScreenMenuStore: ShowSmallScreenMenuStore) => {
            spyOn(showSmallScreenMenuStore, 'updateState').and.callThrough();

            component.closeSidebar();

            expect(showSmallScreenMenuStore.updateState).toHaveBeenCalledWith(false);
        }
    ));
});
