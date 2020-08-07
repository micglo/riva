import { DOCUMENT } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, HostListener, Inject, Input, OnDestroy, OnInit } from '@angular/core';
import { Event, NavigationEnd, Router } from '@angular/router';
import { AuthService, WINDOW } from 'riva-core';
import { combineLatest, Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { LIGHT_DARK_COLORS } from './../../constants/light-dark-colors.const';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { MenuItem } from './../../models/menu-item.model';
import { BodyClassesUpdateService } from './../../services/body-classes-update.service';
import { FullLayoutService } from './../../services/full-layout.service';
import { LayoutService } from './../../services/layout.service';
import { LayoutTemplateStore } from './../../stores/layout-template.store';
import { ShowSmallScreenMenuStore } from './../../stores/show-small-screen-manu.store';

@Component({
    selector: 'lib-full-layout',
    templateUrl: './full-layout.component.html',
    providers: [BodyClassesUpdateService, LayoutService, FullLayoutService],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class FullLayoutComponent implements OnInit, OnDestroy {
    public menuPosition = LayoutMenuPosition;

    private _subscriptions = new Subscription();
    private _smallScreenWidth = 1200;
    private _isScrollTopVisible = false;
    private _layoutTemplate: LayoutTemplate;
    private _isSmallScreenMenuShown: boolean;
    private _resizeTimeout: NodeJS.Timeout;
    private _isSmallScreen: boolean;
    private _bgImage: string;
    private _bgColor: string;
    private _isAuthenticated: boolean;

    public get isSmallScreenMenuShown(): boolean {
        return this._isSmallScreenMenuShown;
    }

    public get isSmallScreen(): boolean {
        return this._isSmallScreen;
    }

    public get bgImage(): string {
        return this._bgImage;
    }

    public get bgColor(): string {
        return this._bgColor;
    }

    public get isScrollTopVisible(): boolean {
        return this._isScrollTopVisible;
    }

    public get isAuthenticated(): boolean {
        return this._isAuthenticated;
    }

    @Input()
    public menuItems: MenuItem[];

    @Input()
    public get layoutTemplate(): LayoutTemplate {
        return this._layoutTemplate;
    }
    public set layoutTemplate(layoutTemplate: LayoutTemplate) {
        this._layoutTemplate = layoutTemplate;
    }

    constructor(
        private layoutTemplateStore: LayoutTemplateStore,
        private showSmallScreenMenuStore: ShowSmallScreenMenuStore,
        private layoutService: LayoutService,
        private fullLayoutService: FullLayoutService,
        private bodyClassesUpdateService: BodyClassesUpdateService,
        private authService: AuthService,
        private router: Router,
        private cdr: ChangeDetectorRef,
        @Inject(WINDOW) private window: Window,
        @Inject(DOCUMENT) private document: Document
    ) {}

    public ngOnInit(): void {
        this._isSmallScreen = this.window.innerWidth < this._smallScreenWidth ? true : false;
        this.layoutTemplateStore.updateState(this._layoutTemplate);
        this.showSmallScreenMenuStore.updateState(false);
        this.subscribeToLayoutTemplateAndShowSidebarAndAuthenticationStateChanges();
        this.subscribeToRouterEventsChanges();
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }

    public scrollToTop(): void {
        const scrollToOptions: ScrollToOptions = {
            top: 0,
            left: 0,
            behavior: 'smooth'
        };
        this.window.scroll(scrollToOptions);
    }

    public onOutsideClick(event: MouseEvent): void {
        const element = event.currentTarget as Element;
        if (!element.classList.contains('toggleSidebarNavbarButton')) {
            this.showSmallScreenMenuStore.updateState(false);
        }
    }

    @HostListener('window:resize', ['$event'])
    public onResize(event: UIEvent): void {
        const timeout = 500;
        const currentWindow = event.currentTarget as Window;
        this._isSmallScreen = currentWindow.innerWidth < this._smallScreenWidth;
        if (this._resizeTimeout) {
            clearTimeout(this._resizeTimeout);
        }
        this._resizeTimeout = setTimeout((() => this.showSmallScreenMenuStore.updateState(false)).bind(this), timeout);
    }

    @HostListener('window:scroll', [])
    public onWindowScroll(): void {
        const offset = this.window.pageYOffset || this.document.documentElement.scrollTop || this.document.body.scrollTop || 0;
        const smallOffset = 20;
        const mediumOffset = 60;
        const largeOffset = 400;

        this._isScrollTopVisible = offset > largeOffset;

        if (offset > smallOffset) {
            this.bodyClassesUpdateService.addPageScrolledClass();
        } else {
            this.bodyClassesUpdateService.removePageScrolledClass();
        }

        if (offset > mediumOffset) {
            this.bodyClassesUpdateService.addNavbarScrolledClass();
        } else {
            this.bodyClassesUpdateService.removeNavbarScrolledClass();
        }
    }

    private subscribeToLayoutTemplateAndShowSidebarAndAuthenticationStateChanges(): void {
        const subscription = combineLatest([
            this.layoutTemplateStore.state$,
            this.showSmallScreenMenuStore.state$,
            this.authService.isAuthenticated$
        ]).subscribe(([layoutTemplate, isSmallScreenMenuShown, isAuthenticated]) => {
            this._layoutTemplate = layoutTemplate;
            this._isSmallScreenMenuShown = isSmallScreenMenuShown;
            this._isAuthenticated = isAuthenticated;
            this.setFullLayout(this._layoutTemplate);
            if (this._isAuthenticated) {
                this.fullLayoutService.setMenuLayout(
                    this._layoutTemplate.menuPosition,
                    this._layoutTemplate.sidebar.collapsed,
                    this._isSmallScreen,
                    this._isSmallScreenMenuShown
                );

                if (layoutTemplate.menuPosition === LayoutMenuPosition.Top) {
                    this.fullLayoutService.setFullLayoutOnNavbarType(this._layoutTemplate.navbarType);
                }
            }
            this.cdr.markForCheck();
        });
        this._subscriptions.add(subscription);
    }

    private subscribeToRouterEventsChanges(): void {
        this.router.events.pipe(filter((event: Event) => event instanceof NavigationEnd)).subscribe(() => {
            if (this._isSmallScreen) {
                this.showSmallScreenMenuStore.updateState(false);
            }
        });
    }

    private setFullLayout(layoutTemplate: LayoutTemplate): void {
        this._bgImage =
            layoutTemplate.sidebar.backgroundImage && layoutTemplate.variant !== LayoutVariant.Transparent
                ? layoutTemplate.sidebar.backgroundImageURL
                : '';
        const blackColorIndex = 7;
        this._bgColor =
            layoutTemplate.variant !== LayoutVariant.Transparent
                ? layoutTemplate.sidebar.backgroundColor
                : LIGHT_DARK_COLORS[blackColorIndex].backgroundColor;

        this.bodyClassesUpdateService.removeTransparentBodyClasses();
        this.layoutService.setLayoutOnVariant(layoutTemplate.variant, this._bgColor);
        this.fullLayoutService.setFullLayoutOnSidebarSize(layoutTemplate.sidebar.size);
    }
}
