import { Injectable } from '@angular/core';
import { LIGHT_DARK_BG_IMAGES } from './../constants/light-dark-bg-images.const';
import { LIGHT_DARK_COLORS } from './../constants/light-dark-colors.const';
import { TRANSPARENT_COLORS_WITH_SHADES } from './../constants/transparent-colors-with-shades.const';
import { TRANSPARENT_COLORS } from './../constants/transparent-colors.const';
import { LayoutNavbarType } from './../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../enums/layout-variant.enum';
import { LightDarkColorType } from './../enums/light-dark-color-type.enum';
import { LayoutServicesModule } from './../layout-services.module';
import { LayoutTemplate } from './../models/layout-template.model';
import { LightDarkColor } from './../models/light-dark-color.model';
import { LightDarkImage } from './../models/light-dark-image.model';
import { TransparentColor } from './../models/transparent-color.model';
import { LayoutTemplateStore } from './../stores/layout-template.store';

@Injectable({
    providedIn: LayoutServicesModule
})
export class CustomizerService {
    constructor(private layoutTemplateStore: LayoutTemplateStore) {}

    public getLightDarkColors(backgroundColor: LayoutSidebarBackgroundColor, type: LightDarkColorType): LightDarkColor[] {
        return LIGHT_DARK_COLORS.filter((color: LightDarkColor) => color.type === type).map((color: LightDarkColor) => {
            color.active = color.backgroundColor === backgroundColor;
            return color;
        });
    }

    public getTransparentColorsWithShades(backgroundColor: LayoutSidebarBackgroundColor): TransparentColor[] {
        return TRANSPARENT_COLORS_WITH_SHADES.map((color: TransparentColor) => {
            color.active = color.backgroundColor === backgroundColor;
            return color;
        });
    }

    public getTransparentColors(backgroundColor: LayoutSidebarBackgroundColor): TransparentColor[] {
        return TRANSPARENT_COLORS.map((color: TransparentColor) => {
            color.active = color.backgroundColor === backgroundColor;
            return color;
        });
    }

    public getLightDarkBgImages(backgroundImageURL: string): LightDarkImage[] {
        return LIGHT_DARK_BG_IMAGES.map((image: LightDarkImage) => {
            image.active = image.src === backgroundImageURL;
            return image;
        });
    }

    public changeLayoutVariant(layoutTemplate: LayoutTemplate, variant: LayoutVariant): void {
        layoutTemplate.variant = variant;
        if (variant === LayoutVariant.Light) {
            const firstImageIndex = 0;
            const manOfSteelColorIndex = 5;
            layoutTemplate.sidebar.backgroundImage = true;
            layoutTemplate.sidebar.backgroundImageURL = LIGHT_DARK_BG_IMAGES[firstImageIndex].src;
            layoutTemplate.sidebar.backgroundColor = LIGHT_DARK_COLORS[manOfSteelColorIndex].backgroundColor;
        } else if (variant === LayoutVariant.Dark) {
            const thirdImageIndex = 2;
            const blackColorIndex = 7;
            layoutTemplate.sidebar.backgroundImage = true;
            layoutTemplate.sidebar.backgroundImageURL = LIGHT_DARK_BG_IMAGES[thirdImageIndex].src;
            layoutTemplate.sidebar.backgroundColor = LIGHT_DARK_COLORS[blackColorIndex].backgroundColor;
        } else {
            const bgGlass1ColorIndex = 0;
            layoutTemplate.sidebar.backgroundImage = false;
            layoutTemplate.sidebar.backgroundImageURL = '';
            layoutTemplate.sidebar.backgroundColor = TRANSPARENT_COLORS_WITH_SHADES[bgGlass1ColorIndex].backgroundColor;
        }

        this.layoutTemplateStore.updateState(layoutTemplate);
    }

    public changeLayoutNavbarType(layoutTemplate: LayoutTemplate, navbarType: LayoutNavbarType): void {
        layoutTemplate.navbarType = navbarType;
        this.layoutTemplateStore.updateState(layoutTemplate);
    }

    public changeLayoutSidebarColor(layoutTemplate: LayoutTemplate, color: LightDarkColor): void {
        layoutTemplate.sidebar.backgroundColor = color.backgroundColor;
        this.layoutTemplateStore.updateState(layoutTemplate);
    }

    public changeLayoutSidebarImage(layoutTemplate: LayoutTemplate, image: LightDarkImage): void {
        layoutTemplate.sidebar.backgroundImageURL = image.src;
        this.layoutTemplateStore.updateState(layoutTemplate);
    }

    public changeLayoutSidebarTransparentColor(layoutTemplate: LayoutTemplate, color: TransparentColor): void {
        layoutTemplate.sidebar.backgroundColor = color.backgroundColor;
        layoutTemplate.sidebar.backgroundImage = false;
        layoutTemplate.sidebar.backgroundImageURL = '';
        this.layoutTemplateStore.updateState(layoutTemplate);
    }

    public displayLayoutSidebarImage(layoutTemplate: LayoutTemplate, event: Event): void {
        layoutTemplate.sidebar.backgroundImage = (event.target as HTMLInputElement).checked;
        this.layoutTemplateStore.updateState(layoutTemplate);
    }

    public changeLayoutSidebarSize(layoutTemplate: LayoutTemplate, size: LayoutSidebarSize): void {
        layoutTemplate.sidebar.size = size;
        this.layoutTemplateStore.updateState(layoutTemplate);
    }
}
