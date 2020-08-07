import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { DeviceDetectorService } from 'ngx-device-detector';
import { Subscription } from 'rxjs';
import { VERTICAL_MENU_ANIMATIONS } from './../../animations/vertical-menu.animations';
import { DARK_LOGO_URL, LIGHT_LOGO_URL } from './../../constants/images.const';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { MenuItem } from './../../models/menu-item.model';
import { LayoutTemplateStore } from './../../stores/layout-template.store';
import { ShowSmallScreenMenuStore } from './../../stores/show-small-screen-manu.store';

@Component({
    selector: 'lib-vertical-menu',
    templateUrl: './vertical-menu.component.html',
    animations: VERTICAL_MENU_ANIMATIONS,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class VerticalMenuComponent implements OnInit, OnDestroy {
    public menuPosition = LayoutMenuPosition;

    private _subscriptions: Subscription = new Subscription();
    private _layoutTemplate: LayoutTemplate;
    private _perfectScrollbarEnabled: boolean;

    public get layoutTemplate(): LayoutTemplate {
        return this._layoutTemplate;
    }

    public get perfectScrollbarEnabled(): boolean {
        return this._perfectScrollbarEnabled;
    }

    public get logoUrl(): string {
        return this._layoutTemplate.variant === LayoutVariant.Light ? DARK_LOGO_URL : LIGHT_LOGO_URL;
    }

    @Input()
    public menuItems: MenuItem[];

    constructor(
        private layoutTemplateStore: LayoutTemplateStore,
        private showSmallScreenMenuStore: ShowSmallScreenMenuStore,
        private deviceDetectorService: DeviceDetectorService,
        private cdr: ChangeDetectorRef
    ) {}

    public ngOnInit(): void {
        this._perfectScrollbarEnabled = this.isMobileOrTabletDevice();
        this.subscribeToLayoutTemplateStateChanges();
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }

    public toggleSidebar(): void {
        this._layoutTemplate.sidebar.collapsed = !this._layoutTemplate.sidebar.collapsed;
        this.layoutTemplateStore.updateState(this._layoutTemplate);
    }

    public closeSidebar(): void {
        this.showSmallScreenMenuStore.updateState(false);
    }

    private subscribeToLayoutTemplateStateChanges(): void {
        const layoutTemplateSubscription = this.layoutTemplateStore.state$.subscribe((state: LayoutTemplate) => {
            this._layoutTemplate = state;
            this.cdr.markForCheck();
        });
        this._subscriptions.add(layoutTemplateSubscription);
    }

    private isMobileOrTabletDevice(): boolean {
        return this.deviceDetectorService.isMobile() || this.deviceDetectorService.isTablet();
    }
}
