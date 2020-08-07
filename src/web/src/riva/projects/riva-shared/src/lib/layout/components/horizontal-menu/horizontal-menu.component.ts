import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../../enums/layout-navbar-type.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { MenuItem } from './../../models/menu-item.model';
import { LayoutTemplateStore } from './../../stores/layout-template.store';

@Component({
    selector: 'lib-horizontal-menu',
    templateUrl: './horizontal-menu.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class HorizontalMenuComponent implements OnInit, OnDestroy {
    public menuPosition = LayoutMenuPosition;
    public navbarType = LayoutNavbarType;

    private _subscriptions = new Subscription();
    private _backgroundColor = '';
    private _layoutTemplate: LayoutTemplate;

    public get layoutTemplate(): LayoutTemplate {
        return this._layoutTemplate;
    }

    public get backgroundColor(): string {
        return this._backgroundColor;
    }

    @Input()
    public menuItems: MenuItem[];

    constructor(private layoutTemplateStore: LayoutTemplateStore, private cdr: ChangeDetectorRef) {}

    public ngOnInit(): void {
        this.subscribeToLayoutTemplateStateChanges();
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }

    private subscribeToLayoutTemplateStateChanges(): void {
        const layoutTemplateSubscription = this.layoutTemplateStore.state$.subscribe((state: LayoutTemplate) => {
            this._layoutTemplate = state;
            this._backgroundColor = state.variant === LayoutVariant.Transparent ? state.sidebar.backgroundColor : '';
            this.cdr.markForCheck();
        });
        this._subscriptions.add(layoutTemplateSubscription);
    }
}
