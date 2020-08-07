import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { DARK_LOGO_URL, LIGHT_LOGO_URL } from './../../constants/images.const';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';

@Component({
    selector: 'lib-navbar-logo',
    templateUrl: './navbar-logo.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavbarLogoComponent {
    public menuPositionEnum = LayoutMenuPosition;

    private _layoutVariant: LayoutVariant;

    public get logoUrl(): string {
        return this._layoutVariant === LayoutVariant.Light ? DARK_LOGO_URL : LIGHT_LOGO_URL;
    }

    @Input()
    public menuPosition: LayoutMenuPosition;

    @Input()
    public set layoutVariant(value: LayoutVariant) {
        this._layoutVariant = value;
    }
}
