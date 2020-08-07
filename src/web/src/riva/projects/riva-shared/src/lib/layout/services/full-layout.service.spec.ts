import { Renderer2 } from '@angular/core';
import { inject, TestBed } from '@angular/core/testing';
import { DeviceDetectorService } from 'ngx-device-detector';
import { LayoutMenuPosition } from './../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../enums/layout-navbar-type.enum';
import { LayoutSidebarSize } from './../enums/layout-sidebar-size.enum';
import { Renderer2Stub } from './../testing/stubs/renderer2.stub';
import { BodyClassesUpdateService } from './body-classes-update.service';
import { FullLayoutService } from './full-layout.service';

describe('FullLayoutService', () => {
    let service: FullLayoutService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [{ provide: Renderer2, useClass: Renderer2Stub }, BodyClassesUpdateService, DeviceDetectorService, FullLayoutService]
        });
        service = TestBed.inject(FullLayoutService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('setFullLayoutOnSidebarSize should call proper BodyClassesUpdateService function to set layout when sidebar size is', () => {
        let sidebarSize: LayoutSidebarSize;

        it('small', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
            sidebarSize = LayoutSidebarSize.Small;
            spyOn(bodyClassesUpdateService, 'updateBodyClassesWhenSidebarSizeIsSmall');

            service.setFullLayoutOnSidebarSize(sidebarSize);

            expect(bodyClassesUpdateService.updateBodyClassesWhenSidebarSizeIsSmall).toHaveBeenCalled();
        }));

        it('medium', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
            sidebarSize = LayoutSidebarSize.Medium;
            spyOn(bodyClassesUpdateService, 'updateBodyClassesWhenSidebarSizeIsMedium').and.callThrough();

            service.setFullLayoutOnSidebarSize(sidebarSize);

            expect(bodyClassesUpdateService.updateBodyClassesWhenSidebarSizeIsMedium).toHaveBeenCalled();
        }));

        it('large', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
            sidebarSize = LayoutSidebarSize.Large;
            spyOn(bodyClassesUpdateService, 'updateBodyClassesWhenSidebarSizeIsLarge').and.callThrough();

            service.setFullLayoutOnSidebarSize(sidebarSize);

            expect(bodyClassesUpdateService.updateBodyClassesWhenSidebarSizeIsLarge).toHaveBeenCalled();
        }));
    });

    describe('setFullLayoutOnNavbarType should call proper BodyClassesUpdateService function to set layout when navbar type is', () => {
        let navbarType: LayoutNavbarType;

        it('fixed', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
            navbarType = LayoutNavbarType.Fixed;
            spyOn(bodyClassesUpdateService, 'updateBodyClassesWhenNavbarTypeIsFixed');

            service.setFullLayoutOnNavbarType(navbarType);

            expect(bodyClassesUpdateService.updateBodyClassesWhenNavbarTypeIsFixed).toHaveBeenCalled();
        }));

        it('static', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
            navbarType = LayoutNavbarType.Static;
            spyOn(bodyClassesUpdateService, 'updateBodyClassesWhenNavbarTypeIsStatic').and.callThrough();

            service.setFullLayoutOnNavbarType(navbarType);

            expect(bodyClassesUpdateService.updateBodyClassesWhenNavbarTypeIsStatic).toHaveBeenCalled();
        }));
    });

    describe('setMenuLayout should set menu layout by calling proper BodyClassesUpdateService function when ', () => {
        let menuPosition: LayoutMenuPosition;
        let isSmallScreen: boolean;
        let isSmallScreenMenuShown: boolean;
        const isSidebarCollapsed = false;

        describe('menu position is top and screen is', () => {
            beforeEach(() => {
                menuPosition = LayoutMenuPosition.Top;
                isSmallScreenMenuShown = false;
            });

            it('small', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
                isSmallScreen = true;
                spyOn(bodyClassesUpdateService, 'removeBlankPageClass');
                spyOn(bodyClassesUpdateService, 'removeLayoutMenuClasses');
                spyOn(bodyClassesUpdateService, 'updateBodyClassesUpdateWhenMenuPositionIsTop');
                spyOn(bodyClassesUpdateService, 'updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsSmall');

                service.setMenuLayout(menuPosition, isSidebarCollapsed, isSmallScreen, isSmallScreenMenuShown);

                expect(bodyClassesUpdateService.removeBlankPageClass).toHaveBeenCalled();
                expect(bodyClassesUpdateService.removeLayoutMenuClasses).toHaveBeenCalled();
                expect(bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsTop).toHaveBeenCalled();
                expect(bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsSmall).toHaveBeenCalledWith(
                    isSmallScreenMenuShown
                );
            }));

            it('not small', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
                isSmallScreen = false;
                spyOn(bodyClassesUpdateService, 'removeBlankPageClass');
                spyOn(bodyClassesUpdateService, 'removeLayoutMenuClasses');
                spyOn(bodyClassesUpdateService, 'updateBodyClassesUpdateWhenMenuPositionIsTop');
                spyOn(bodyClassesUpdateService, 'updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsLarge');

                service.setMenuLayout(menuPosition, isSidebarCollapsed, isSmallScreen, isSmallScreenMenuShown);

                expect(bodyClassesUpdateService.removeBlankPageClass).toHaveBeenCalled();
                expect(bodyClassesUpdateService.removeLayoutMenuClasses).toHaveBeenCalled();
                expect(bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsTop).toHaveBeenCalled();
                expect(bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsLarge).toHaveBeenCalled();
            }));
        });

        describe('menu position is side and screen is', () => {
            beforeEach(() => {
                menuPosition = LayoutMenuPosition.Side;
                isSmallScreenMenuShown = true;
            });

            it('small', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
                isSmallScreen = true;
                spyOn(bodyClassesUpdateService, 'removeBlankPageClass');
                spyOn(bodyClassesUpdateService, 'removeLayoutMenuClasses');
                spyOn(bodyClassesUpdateService, 'updateBodyClassesUpdateWhenMenuPositionIsSide');
                spyOn(bodyClassesUpdateService, 'updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsSmall');

                service.setMenuLayout(menuPosition, isSidebarCollapsed, isSmallScreen, isSmallScreenMenuShown);

                expect(bodyClassesUpdateService.removeBlankPageClass).toHaveBeenCalled();
                expect(bodyClassesUpdateService.removeLayoutMenuClasses).toHaveBeenCalled();
                expect(bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsSide).toHaveBeenCalled();
                expect(bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsSmall).toHaveBeenCalledWith(
                    isSmallScreenMenuShown
                );
            }));

            it('not small', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
                isSmallScreen = false;
                spyOn(bodyClassesUpdateService, 'removeBlankPageClass');
                spyOn(bodyClassesUpdateService, 'removeLayoutMenuClasses');
                spyOn(bodyClassesUpdateService, 'updateBodyClassesUpdateWhenMenuPositionIsSide');
                spyOn(bodyClassesUpdateService, 'updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsLarge');

                service.setMenuLayout(menuPosition, isSidebarCollapsed, isSmallScreen, isSmallScreenMenuShown);

                expect(bodyClassesUpdateService.removeBlankPageClass).toHaveBeenCalled();
                expect(bodyClassesUpdateService.removeLayoutMenuClasses).toHaveBeenCalled();
                expect(bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsSide).toHaveBeenCalled();
                expect(bodyClassesUpdateService.updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsLarge).toHaveBeenCalledWith(
                    isSidebarCollapsed
                );
            }));
        });

        it('device is mobile or tablet', inject(
            [DeviceDetectorService, BodyClassesUpdateService],
            (deviceDetectorService: DeviceDetectorService, bodyClassesUpdateService: BodyClassesUpdateService) => {
                menuPosition = LayoutMenuPosition.Top;
                isSmallScreen = true;
                isSmallScreenMenuShown = true;
                spyOn(deviceDetectorService, 'isMobile').and.returnValue(true);
                spyOn(deviceDetectorService, 'isTablet').and.returnValue(false);
                spyOn(bodyClassesUpdateService, 'addOverflowHiddenClass');

                service.setMenuLayout(menuPosition, isSidebarCollapsed, isSmallScreen, isSmallScreenMenuShown);

                expect(bodyClassesUpdateService.addOverflowHiddenClass).toHaveBeenCalled();
            }
        ));
    });
});
