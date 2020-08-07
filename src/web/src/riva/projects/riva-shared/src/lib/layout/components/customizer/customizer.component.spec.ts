import { async, ComponentFixture, inject, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { SIDEBAR_BG_1_URL } from './../../constants/images.const';
import { LIGHT_DARK_BG_IMAGES } from './../../constants/light-dark-bg-images.const';
import { LIGHT_DARK_COLORS } from './../../constants/light-dark-colors.const';
import { TRANSPARENT_COLORS_WITH_SHADES } from './../../constants/transparent-colors-with-shades.const';
import { TRANSPARENT_COLORS } from './../../constants/transparent-colors.const';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LightDarkColorType } from './../../enums/light-dark-color-type.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { LightDarkColor } from './../../models/light-dark-color.model';
import { LightDarkImage } from './../../models/light-dark-image.model';
import { TransparentColor } from './../../models/transparent-color.model';
import { CustomizerService } from './../../services/customizer.service';
import { LayoutTemplateStore } from './../../stores/layout-template.store';
import { CustomizerComponent } from './customizer.component';

describe('CustomizerComponent', () => {
    let component: CustomizerComponent;
    let fixture: ComponentFixture<CustomizerComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [CustomizerComponent],
            providers: [LayoutTemplateStore, CustomizerService]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(CustomizerComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('ngOnInit should set properties', inject(
        [LayoutTemplateStore, CustomizerService],
        (layoutTemplateStore: LayoutTemplateStore, customizerService: CustomizerService) => {
            const layoutTemplate: LayoutTemplate = {
                variant: LayoutVariant.Light,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };
            const gradientLightDarkColorType = LightDarkColorType.Gradient;
            const solidLightDarkColorType = LightDarkColorType.Solid;
            const lightDarkColorsForGradient = LIGHT_DARK_COLORS.filter(
                (color: LightDarkColor) => color.type === gradientLightDarkColorType
            ).map((color: LightDarkColor) => {
                color.active = color.backgroundColor === layoutTemplate.sidebar.backgroundColor;
                return color;
            });
            const lightDarkColorsForSolid = LIGHT_DARK_COLORS.filter((color: LightDarkColor) => color.type === solidLightDarkColorType).map(
                (color: LightDarkColor) => {
                    color.active = color.backgroundColor === layoutTemplate.sidebar.backgroundColor;
                    return color;
                }
            );
            const transparentColorsWithShades = TRANSPARENT_COLORS_WITH_SHADES.map((color: TransparentColor) => {
                color.active = color.backgroundColor === layoutTemplate.sidebar.backgroundColor;
                return color;
            });
            const transparentColors = TRANSPARENT_COLORS.map((color: TransparentColor) => {
                color.active = color.backgroundColor === layoutTemplate.sidebar.backgroundColor;
                return color;
            });
            const lightDarkBgImages = LIGHT_DARK_BG_IMAGES.map((image: LightDarkImage) => {
                image.active = image.src === layoutTemplate.sidebar.backgroundImageURL;
                return image;
            });

            spyOnProperty(layoutTemplateStore, 'state$', 'get').and.returnValue(of(layoutTemplate));
            spyOn(customizerService, 'getLightDarkColors')
                .withArgs(layoutTemplate.sidebar.backgroundColor, gradientLightDarkColorType)
                .and.returnValue(lightDarkColorsForGradient)
                .withArgs(layoutTemplate.sidebar.backgroundColor, solidLightDarkColorType)
                .and.returnValue(lightDarkColorsForSolid);
            spyOn(customizerService, 'getTransparentColorsWithShades').and.returnValue(transparentColorsWithShades);
            spyOn(customizerService, 'getTransparentColors').and.returnValue(transparentColors);
            spyOn(customizerService, 'getLightDarkBgImages').and.returnValue(lightDarkBgImages);

            component.ngOnInit();

            expect(component.layoutTemplate).toEqual(layoutTemplate);
            expect(component.lightDarkGradientColors).toEqual(lightDarkColorsForGradient);
            expect(component.lightDarkSolidColors).toEqual(lightDarkColorsForSolid);
            expect(component.transparentColors).toEqual(transparentColors);
            expect(component.transparentColorsWithShades).toEqual(transparentColorsWithShades);
            expect(component.lightDarkBgImages).toEqual(lightDarkBgImages);
        }
    ));

    it('toggleCustomizer should call layoutTemplateStore.updateState with opposite value', inject(
        [LayoutTemplateStore],
        (layoutTemplateStore: LayoutTemplateStore) => {
            const layoutTemplate: LayoutTemplate = {
                variant: LayoutVariant.Light,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };
            const expectedLayoutTemplate: LayoutTemplate = {
                variant: LayoutVariant.Light,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: true,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };

            spyOnProperty(layoutTemplateStore, 'state$', 'get').and.returnValue(of(layoutTemplate));
            spyOn(layoutTemplateStore, 'updateState').and.callThrough();
            component.ngOnInit();

            component.toggleCustomizer();

            expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
        }
    ));

    it('closeCustomizer should call layoutTemplateStore.updateState with isCustomizerOpen equals false', inject(
        [LayoutTemplateStore],
        (layoutTemplateStore: LayoutTemplateStore) => {
            const layoutTemplate: LayoutTemplate = {
                variant: LayoutVariant.Light,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: true,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };
            const expectedLayoutTemplate: LayoutTemplate = {
                variant: LayoutVariant.Light,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };

            spyOnProperty(layoutTemplateStore, 'state$', 'get').and.returnValue(of(layoutTemplate));
            spyOn(layoutTemplateStore, 'updateState').and.callThrough();
            component.ngOnInit();

            component.closeCustomizer();

            expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
        }
    ));
});
