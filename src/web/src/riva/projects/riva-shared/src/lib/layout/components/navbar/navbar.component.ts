import { ChangeDetectionStrategy, ChangeDetectorRef, Component, HostListener, Inject, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, WINDOW } from 'riva-core';
import { Observable, Subscription } from 'rxjs';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../../enums/layout-navbar-type.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { LayoutTemplateStore } from './../../stores/layout-template.store';
import { ShowSmallScreenMenuStore } from './../../stores/show-small-screen-manu.store';

@Component({
    selector: 'lib-navbar',
    templateUrl: './navbar.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavbarComponent implements OnInit, OnDestroy {
    public menuPosition = LayoutMenuPosition;
    public navbarType = LayoutNavbarType;

    private _subscriptions = new Subscription();
    private _smallScreenWidth = 1200;
    private _layoutTemplate: LayoutTemplate;
    private _backgroundColor: string;
    private _isSmallScreenMenuShown: boolean;
    private _isSmallScreen: boolean;

    public get layoutTemplate(): LayoutTemplate {
        return this._layoutTemplate;
    }

    public get backgroundColor(): string {
        return this._backgroundColor;
    }

    public get isSmallScreen(): boolean {
        return this._isSmallScreen;
    }

    public get isAuthenticated$(): Observable<boolean> {
        return this.authService.isAuthenticated$;
    }

    constructor(
        private layoutTemplateStore: LayoutTemplateStore,
        private showSmallScreenMenuStore: ShowSmallScreenMenuStore,
        private cdr: ChangeDetectorRef,
        private authService: AuthService,
        private router: Router,
        @Inject(WINDOW) private window: Window
    ) {}

    public ngOnInit(): void {
        this.subscribeToLayoutTemplateStateChanges();
        this.subscribeToShowSmallScreenMenuStateChanges();
        this._isSmallScreen = this.window.innerWidth < this._smallScreenWidth;
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }

    @HostListener('window:resize', ['$event'])
    public onResize(event: UIEvent): void {
        const currentWindow = event.currentTarget as Window;
        this._isSmallScreen = currentWindow.innerWidth < this._smallScreenWidth;
    }

    public toggleSidebar(): void {
        this.showSmallScreenMenuStore.updateState(!this._isSmallScreenMenuShown);
    }

    public login(): void {
        this.authService.login(this.router.url);
    }

    private subscribeToLayoutTemplateStateChanges(): void {
        const layoutTemplateSubscription = this.layoutTemplateStore.state$.subscribe((state: LayoutTemplate) => {
            this._layoutTemplate = state;
            this._backgroundColor =
                this._layoutTemplate.variant === LayoutVariant.Transparent ? this._layoutTemplate.sidebar.backgroundColor : '';
            this.cdr.markForCheck();
        });
        this._subscriptions.add(layoutTemplateSubscription);
    }

    private subscribeToShowSmallScreenMenuStateChanges(): void {
        const isSmallScreenMenuSubscription = this.showSmallScreenMenuStore.state$.subscribe(
            state => (this._isSmallScreenMenuShown = state)
        );
        this._subscriptions.add(isSmallScreenMenuSubscription);
    }
}
