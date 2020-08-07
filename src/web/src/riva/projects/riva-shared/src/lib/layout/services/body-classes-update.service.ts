import { DOCUMENT } from '@angular/common';
import { Inject, Injectable, Renderer2 } from '@angular/core';
import { TRANSPARENT_COLORS_WITH_SHADES } from './../constants/transparent-colors-with-shades.const';
import { TRANSPARENT_COLORS } from './../constants/transparent-colors.const';
import { LayoutContentBodyClass } from './../enums/layout-content-body-class.enum';
import { LayoutMenuBodyClass } from './../enums/layout-menu-body-class.enum';
import { LayoutNavbarBodyClass } from './../enums/layout-navbar-body-class.enum';
import { LayoutSidebarSize } from './../enums/layout-sidebar-size.enum';
import { LayoutVariantBodyClass } from './../enums/layout-variant-body-class.num';
import { TransparentColor } from './../models/transparent-color.model';

@Injectable()
export class BodyClassesUpdateService {
    constructor(private renderer: Renderer2, @Inject(DOCUMENT) private document: Document) {}

    public removeTransparentBodyClasses(): void {
        TRANSPARENT_COLORS.forEach((color: TransparentColor) => this.renderer.removeClass(this.document.body, color.backgroundColor));
        TRANSPARENT_COLORS_WITH_SHADES.forEach((color: TransparentColor) =>
            this.renderer.removeClass(this.document.body, color.backgroundColor)
        );
    }

    public updateBodyClassesWhenLayoutVariantIsLight(): void {
        this.renderer.removeClass(this.document.body, LayoutVariantBodyClass.LayoutDark);
        this.renderer.removeClass(this.document.body, LayoutVariantBodyClass.LayoutTransparent);
    }

    public updateBodyClassesWhenLayoutVariantIsDark(): void {
        this.renderer.removeClass(this.document.body, LayoutVariantBodyClass.LayoutTransparent);
        this.renderer.addClass(this.document.body, LayoutVariantBodyClass.LayoutDark);
    }

    public updateBodyClassesWhenLayoutVariantIsTransparent(bgColor: string): void {
        this.renderer.addClass(this.document.body, LayoutVariantBodyClass.LayoutDark);
        this.renderer.addClass(this.document.body, LayoutVariantBodyClass.LayoutTransparent);
        this.renderer.addClass(this.document.body, bgColor);
    }

    public updateBodyClassesWhenSidebarSizeIsSmall(): void {
        this.renderer.removeClass(this.document.body, LayoutSidebarSize.Large);
        this.renderer.addClass(this.document.body, LayoutSidebarSize.Small);
    }

    public updateBodyClassesWhenSidebarSizeIsMedium(): void {
        this.renderer.removeClass(this.document.body, LayoutSidebarSize.Small);
        this.renderer.removeClass(this.document.body, LayoutSidebarSize.Large);
    }

    public updateBodyClassesWhenSidebarSizeIsLarge(): void {
        this.renderer.removeClass(this.document.body, LayoutSidebarSize.Small);
        this.renderer.addClass(this.document.body, LayoutSidebarSize.Large);
    }

    public updateBodyClassesWhenNavbarTypeIsStatic(): void {
        this.renderer.removeClass(this.document.body, LayoutNavbarBodyClass.NavbarSticky);
        this.renderer.addClass(this.document.body, LayoutNavbarBodyClass.NavbarStatic);
    }

    public updateBodyClassesWhenNavbarTypeIsFixed(): void {
        this.removeNavbarStaticClass();
        this.renderer.addClass(this.document.body, LayoutNavbarBodyClass.NavbarSticky);
    }

    public addBlankPageClass(): void {
        this.renderer.addClass(this.document.body, 'blank-page');
    }

    public removeBlankPageClass(): void {
        this.renderer.removeClass(this.document.body, 'blank-page');
    }

    public removeLayoutMenuClasses(): void {
        const layoutMenuClasses = Object.keys(LayoutMenuBodyClass).map((key: string) => LayoutMenuBodyClass[key]) as string[];
        layoutMenuClasses.forEach((layoutMenuClass: string) => this.renderer.removeClass(this.document.body, layoutMenuClass));
    }

    public updateBodyClassesUpdateWhenMenuPositionIsTop(): void {
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.HorizontalLayout);
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.HorizontalMenuPadding);
    }

    public updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsSmall(isSmallScreenMenuShown: boolean): void {
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.FixedNavbar);
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.VerticalLayout);
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.VerticalOverlayMenu);

        if (isSmallScreenMenuShown) {
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.MenuExpanded);
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.MenuOpen);
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.VerticalMenu);
        } else {
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.MenuHide);
        }
    }

    public updateBodyClassesUpdateWhenMenuPositionIsTopAndScreenIsLarge(): void {
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.HorizontalMenu);
        this.renderer.setAttribute(this.document.body, 'data-menu', LayoutMenuBodyClass.HorizontalMenu);
    }

    public updateBodyClassesUpdateWhenMenuPositionIsSide(): void {
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.VerticalLayout);
    }

    public updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsSmall(isSmallScreenMenuShown: boolean): void {
        if (isSmallScreenMenuShown) {
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.MenuExpanded);
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.MenuOpen);
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.OverflowHidden);
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.VerticalMenu);
        } else {
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.MenuHide);
        }
    }

    public updateBodyClassesUpdateWhenMenuPositionIsSideAndScreenIsLarge(isSidebarCollapsed: boolean): void {
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.VerticalMenu);
        this.renderer.setAttribute(this.document.body, 'data-menu', LayoutMenuBodyClass.VerticalMenu);

        if (isSidebarCollapsed) {
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.NavCollapsed);
        } else {
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.MenuExpanded);
            this.renderer.addClass(this.document.body, LayoutMenuBodyClass.MenuOpen);
        }
    }

    public addOverflowHiddenClass(): void {
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.OverflowHidden);
    }

    public addPageScrolledClass(): void {
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.PageScrolled);
    }

    public removePageScrolledClass(): void {
        this.renderer.removeClass(this.document.body, LayoutMenuBodyClass.PageScrolled);
    }

    public addNavbarScrolledClass(): void {
        this.renderer.addClass(this.document.body, LayoutMenuBodyClass.NavbarScrolled);
    }

    public removeNavbarScrolledClass(): void {
        this.renderer.removeClass(this.document.body, LayoutMenuBodyClass.NavbarScrolled);
    }

    public addAuthPageClass(): void {
        this.renderer.addClass(this.document.body, LayoutContentBodyClass.AuthPage);
    }

    public removeAuthPageClass(): void {
        this.renderer.removeClass(this.document.body, LayoutContentBodyClass.AuthPage);
    }

    public removeMenuExpandedClass(): void {
        this.renderer.removeClass(this.document.body, LayoutMenuBodyClass.MenuExpanded);
    }

    public removeNavbarStaticClass(): void {
        this.renderer.removeClass(this.document.body, LayoutNavbarBodyClass.NavbarStatic);
    }

    public removeMenuOpenClass(): void {
        this.renderer.removeClass(this.document.body, LayoutMenuBodyClass.MenuOpen);
    }
}
