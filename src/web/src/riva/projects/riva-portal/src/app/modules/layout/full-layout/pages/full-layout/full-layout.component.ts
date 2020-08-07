import { ChangeDetectionStrategy, Component } from '@angular/core';
import { LayoutTemplate, MenuItem } from 'riva-shared';
import { LAYOUT_TEMPLATE } from './../../common/contants/layout-template.const';
import { MENU_ITEMS } from './../../common/contants/menu-items.const';

@Component({
    selector: 'app-full-layout',
    templateUrl: './full-layout.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class FullLayoutComponent {
    private _layoutTemplate = LAYOUT_TEMPLATE;
    private _menuItems = MENU_ITEMS;

    public get layoutTemplate(): LayoutTemplate {
        return this._layoutTemplate;
    }

    public get menuItems(): MenuItem[] {
        return this._menuItems;
    }
}
