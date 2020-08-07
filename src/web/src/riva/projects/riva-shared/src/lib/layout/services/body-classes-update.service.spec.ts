import { DOCUMENT } from '@angular/common';
import { Renderer2 } from '@angular/core';
import { inject, TestBed } from '@angular/core/testing';
import { TRANSPARENT_COLORS_WITH_SHADES } from './../constants/transparent-colors-with-shades.const';
import { TRANSPARENT_COLORS } from './../constants/transparent-colors.const';
import { LayoutMenuBodyClass } from './../enums/layout-menu-body-class.enum';
import { LayoutNavbarBodyClass } from './../enums/layout-navbar-body-class.enum';
import { LayoutSidebarSize } from './../enums/layout-sidebar-size.enum';
import { LayoutVariantBodyClass } from './../enums/layout-variant-body-class.num';
import { TransparentColor } from './../models/transparent-color.model';
import { DOCUMENT_MOCK } from './../testing/mocks/document.mock';
import { Renderer2Stub } from './../testing/stubs/renderer2.stub';
import { BodyClassesUpdateService } from './body-classes-update.service';

describe('BodyClassesUpdateService', () => {
    let service: BodyClassesUpdateService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                { provide: Renderer2, useClass: Renderer2Stub },
                { provide: DOCUMENT, useValue: DOCUMENT_MOCK },
                BodyClassesUpdateService
            ]
        });
        service = TestBed.inject(BodyClassesUpdateService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('removeTransparentBodyClasses should remove transparent body classes', inject([Renderer2], (renderer: Renderer2) => {
        spyOn(renderer, 'removeClass').and.callThrough();

        service.removeTransparentBodyClasses();

        TRANSPARENT_COLORS.forEach((color: TransparentColor) =>
            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, color.backgroundColor)
        );
        TRANSPARENT_COLORS_WITH_SHADES.forEach((color: TransparentColor) =>
            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, color.backgroundColor)
        );
    }));

    it('updateBodyClassesWhenLayoutVariantIsLight should remove layout-dark and layout-transparent classes from body', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            spyOn(renderer, 'removeClass').and.callThrough();

            service.updateBodyClassesWhenLayoutVariantIsLight();

            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutVariantBodyClass.LayoutDark);
            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutVariantBodyClass.LayoutTransparent);
        }
    ));

    it('updateBodyClassesWhenLayoutVariantIsDark should remove layout-transparent and add layout-dark classes', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            spyOn(renderer, 'removeClass').and.callThrough();
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesWhenLayoutVariantIsDark();

            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutVariantBodyClass.LayoutTransparent);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutVariantBodyClass.LayoutDark);
        }
    ));

    it('updateBodyClassesWhenLayoutVariantIsTransparent should add layout-dark, layout-transparent and background color classes', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            const bgColor = 'black';
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesWhenLayoutVariantIsTransparent(bgColor);

            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutVariantBodyClass.LayoutDark);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutVariantBodyClass.LayoutTransparent);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, bgColor);
        }
    ));

    it('updateBodyClassesWhenSidebarSizeIsSmall should remove sidebar-lg and add sidebar-sm classes', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            spyOn(renderer, 'removeClass').and.callThrough();
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesWhenSidebarSizeIsSmall();

            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutSidebarSize.Large);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutSidebarSize.Small);
        }
    ));

    it('updateBodyClassesWhenSidebarSizeIsMedium should remove sidebar-sm and sidebar-lg classes', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            spyOn(renderer, 'removeClass').and.callThrough();

            service.updateBodyClassesWhenSidebarSizeIsMedium();

            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutSidebarSize.Small);
            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutSidebarSize.Large);
        }
    ));

    it('updateBodyClassesWhenSidebarSizeIsLarge should remove sidebar-sm and add sidebar-lg classes', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            spyOn(renderer, 'removeClass').and.callThrough();
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesWhenSidebarSizeIsLarge();

            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutSidebarSize.Small);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutSidebarSize.Large);
        }
    ));

    it('updateBodyClassesWhenNavbarTypeIsStatic should remove navbar-static and add navbar-sticky classes', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            spyOn(renderer, 'removeClass').and.callThrough();
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesWhenNavbarTypeIsStatic();

            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutNavbarBodyClass.NavbarSticky);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutNavbarBodyClass.NavbarStatic);
        }
    ));

    it('updateBodyClassesWhenNavbarTypeIsFixed should remove navbar-sticky and add navbar-static classes', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            spyOn(renderer, 'removeClass').and.callThrough();
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesWhenNavbarTypeIsFixed();

            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutNavbarBodyClass.NavbarStatic);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutNavbarBodyClass.NavbarSticky);
        }
    ));

    it('removeBlankPageClass should remove blank-page class', inject([Renderer2], (renderer: Renderer2) => {
        spyOn(renderer, 'removeClass').and.callThrough();

        service.removeBlankPageClass();

        expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, 'blank-page');
    }));

    it('removeLayoutMenuClasses should remove layout menu body classes', inject([Renderer2], (renderer: Renderer2) => {
        spyOn(renderer, 'removeClass').and.callThrough();

        service.removeLayoutMenuClasses();

        const layoutMenuClasses = Object.keys(LayoutMenuBodyClass).map((key: string) => LayoutMenuBodyClass[key]) as string[];
        layoutMenuClasses.forEach((layoutMenuClass: string) =>
            expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, layoutMenuClass)
        );
    }));

    it('updateBodyClassesUpdateWhenMenuPositionIsTop should add horizontal-layout and horizontal-menu-padding classes', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesUpdateWhenMenuPositionIsTop();

            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.HorizontalLayout);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.HorizontalMenuPadding);
        }
    ));

    describe('updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsSmall should add proper classes when small screen menu is', () => {
        let isSmallScreenMenuShown: boolean;

        it('shown', inject([Renderer2], (renderer: Renderer2) => {
            isSmallScreenMenuShown = true;
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsSmall(isSmallScreenMenuShown);

            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.FixedNavbar);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.VerticalLayout);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.VerticalOverlayMenu);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.MenuExpanded);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.MenuOpen);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.VerticalMenu);
        }));

        it('hidden', inject([Renderer2], (renderer: Renderer2) => {
            isSmallScreenMenuShown = false;
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsSmall(isSmallScreenMenuShown);

            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.FixedNavbar);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.VerticalLayout);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.VerticalOverlayMenu);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.MenuHide);
        }));
    });

    it('updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsLarge should add horizontal-menu class and set data-menu attribute', inject(
        [Renderer2],
        (renderer: Renderer2) => {
            spyOn(renderer, 'addClass').and.callThrough();
            spyOn(renderer, 'setAttribute').and.callThrough();

            service.updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsLarge();

            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.HorizontalMenu);
            expect(renderer.setAttribute).toHaveBeenCalledWith(DOCUMENT_MOCK.body, 'data-menu', LayoutMenuBodyClass.HorizontalMenu);
        }
    ));

    it('updateBodyClassesUpdateWhenMenuPositionIsSide should add vertical-layout class', inject([Renderer2], (renderer: Renderer2) => {
        spyOn(renderer, 'addClass').and.callThrough();

        service.updateBodyClassesUpdateWhenMenuPositionIsSide();

        expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.VerticalLayout);
    }));

    describe('updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsSmall should add proper class when small screen menu is', () => {
        let isSmallScreenMenuShown: boolean;

        it('shown', inject([Renderer2], (renderer: Renderer2) => {
            isSmallScreenMenuShown = true;
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsSmall(isSmallScreenMenuShown);

            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.MenuExpanded);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.MenuOpen);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.OverflowHidden);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.VerticalMenu);
        }));

        it('hidden', inject([Renderer2], (renderer: Renderer2) => {
            isSmallScreenMenuShown = false;
            spyOn(renderer, 'addClass').and.callThrough();

            service.updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsSmall(isSmallScreenMenuShown);

            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.MenuHide);
        }));
    });

    describe('updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsLarge should add proper class when sidebar is', () => {
        let isCollapsed: boolean;

        it('collapsed', inject([Renderer2], (renderer: Renderer2) => {
            isCollapsed = true;
            spyOn(renderer, 'addClass').and.callThrough();
            spyOn(renderer, 'setAttribute').and.callThrough();

            service.updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsLarge(isCollapsed);

            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.VerticalMenu);
            expect(renderer.setAttribute).toHaveBeenCalledWith(DOCUMENT_MOCK.body, 'data-menu', LayoutMenuBodyClass.VerticalMenu);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.NavCollapsed);
        }));

        it('not collapsed', inject([Renderer2], (renderer: Renderer2) => {
            isCollapsed = false;
            spyOn(renderer, 'addClass').and.callThrough();
            spyOn(renderer, 'setAttribute').and.callThrough();

            service.updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsLarge(isCollapsed);

            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.VerticalMenu);
            expect(renderer.setAttribute).toHaveBeenCalledWith(DOCUMENT_MOCK.body, 'data-menu', LayoutMenuBodyClass.VerticalMenu);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.MenuExpanded);
            expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.MenuOpen);
        }));
    });

    it('addOverflowHiddenClass should add overflow-hidden class', inject([Renderer2], (renderer: Renderer2) => {
        spyOn(renderer, 'addClass').and.callThrough();

        service.addOverflowHiddenClass();

        expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.OverflowHidden);
    }));

    it('addPageScrolledClass should add page-scrolled class', inject([Renderer2], (renderer: Renderer2) => {
        spyOn(renderer, 'addClass').and.callThrough();

        service.addPageScrolledClass();

        expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.PageScrolled);
    }));

    it('removePageScrolledClass should remove page-scrolled class', inject([Renderer2], (renderer: Renderer2) => {
        spyOn(renderer, 'removeClass').and.callThrough();

        service.removePageScrolledClass();

        expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.PageScrolled);
    }));

    it('addNavbarScrolledClass should add navbar-scrolled class', inject([Renderer2], (renderer: Renderer2) => {
        spyOn(renderer, 'addClass').and.callThrough();

        service.addNavbarScrolledClass();

        expect(renderer.addClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.NavbarScrolled);
    }));

    it('removeNavbarScrolledClass should remove navbar-scrolled class', inject([Renderer2], (renderer: Renderer2) => {
        spyOn(renderer, 'removeClass').and.callThrough();

        service.removeNavbarScrolledClass();

        expect(renderer.removeClass).toHaveBeenCalledWith(DOCUMENT_MOCK.body, LayoutMenuBodyClass.NavbarScrolled);
    }));
});
