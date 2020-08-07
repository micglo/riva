import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../../enums/layout-navbar-type.enum';
import { LayoutSidebarSize } from './../../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LightDarkColorType } from './../../enums/light-dark-color-type.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { LightDarkColor } from './../../models/light-dark-color.model';
import { LightDarkImage } from './../../models/light-dark-image.model';
import { TransparentColor } from './../../models/transparent-color.model';
import { CustomizerService } from './../../services/customizer.service';
import { LayoutTemplateStore } from './../../stores/layout-template.store';

@Component({
    selector: 'lib-customizer',
    templateUrl: './customizer.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomizerComponent implements OnInit, OnDestroy {
    public variant = LayoutVariant;
    public menuPosition = LayoutMenuPosition;
    public navbarType = LayoutNavbarType;
    public sidebarSize = LayoutSidebarSize;

    private _subscriptions = new Subscription();
    private _layoutTemplate: LayoutTemplate;
    private _lightDarkGradientColors: LightDarkColor[];
    private _lightDarkSolidColors: LightDarkColor[];
    private _transparentColors: TransparentColor[];
    private _transparentColorsWithShades: TransparentColor[];
    private _lightDarkBgImages: LightDarkImage[];

    public get layoutTemplate(): LayoutTemplate {
        return this._layoutTemplate;
    }

    public get lightDarkGradientColors(): LightDarkColor[] {
        return this._lightDarkGradientColors;
    }

    public get lightDarkSolidColors(): LightDarkColor[] {
        return this._lightDarkSolidColors;
    }

    public get transparentColors(): TransparentColor[] {
        return this._transparentColors;
    }

    public get transparentColorsWithShades(): TransparentColor[] {
        return this._transparentColorsWithShades;
    }

    public get lightDarkBgImages(): LightDarkImage[] {
        return this._lightDarkBgImages;
    }

    @Input() public isSmallScreen: boolean;

    constructor(private layoutTemplateStore: LayoutTemplateStore, public customizerService: CustomizerService) {}

    public ngOnInit(): void {
        this.subscribeToLayoutTemplateStateChanges();
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }

    public toggleCustomizer(): void {
        this._layoutTemplate.isCustomizerOpen = !this._layoutTemplate.isCustomizerOpen;
        this.layoutTemplateStore.updateState(this._layoutTemplate);
    }

    public closeCustomizer(): void {
        this._layoutTemplate.isCustomizerOpen = false;
        this.layoutTemplateStore.updateState(this._layoutTemplate);
    }

    private subscribeToLayoutTemplateStateChanges(): void {
        const layoutTemplateSubscription = this.layoutTemplateStore.state$.subscribe((state: LayoutTemplate) => {
            this._layoutTemplate = state;
            this._lightDarkGradientColors = this.customizerService.getLightDarkColors(
                this._layoutTemplate.sidebar.backgroundColor,
                LightDarkColorType.Gradient
            );
            this._lightDarkSolidColors = this.customizerService.getLightDarkColors(
                this._layoutTemplate.sidebar.backgroundColor,
                LightDarkColorType.Solid
            );
            this._transparentColorsWithShades = this.customizerService.getTransparentColorsWithShades(
                this._layoutTemplate.sidebar.backgroundColor
            );
            this._transparentColors = this.customizerService.getTransparentColors(this._layoutTemplate.sidebar.backgroundColor);
            this._lightDarkBgImages = this.customizerService.getLightDarkBgImages(this._layoutTemplate.sidebar.backgroundColor);
        });
        this._subscriptions.add(layoutTemplateSubscription);
    }
}
