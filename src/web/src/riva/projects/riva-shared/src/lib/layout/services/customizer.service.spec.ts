import { inject, TestBed } from '@angular/core/testing';
import { SIDEBAR_BG_1_URL, SIDEBAR_BG_3_URL, SIDEBAR_BG_4_URL } from './../constants/images.const';
import { LIGHT_DARK_BG_IMAGES } from './../constants/light-dark-bg-images.const';
import { LIGHT_DARK_COLORS } from './../constants/light-dark-colors.const';
import { TRANSPARENT_COLORS_WITH_SHADES } from './../constants/transparent-colors-with-shades.const';
import { TRANSPARENT_COLORS } from './../constants/transparent-colors.const';
import { LayoutMenuPosition } from './../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../enums/layout-variant.enum';
import { LightDarkColorType } from './../enums/light-dark-color-type.enum';
import { LayoutTemplate } from './../models/layout-template.model';
import { LightDarkColor } from './../models/light-dark-color.model';
import { LightDarkImage } from './../models/light-dark-image.model';
import { TransparentColor } from './../models/transparent-color.model';
import { LayoutTemplateStore } from './../stores/layout-template.store';
import { CustomizerService } from './customizer.service';

describe('CustomizerService', () => {
    let service: CustomizerService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [LayoutTemplateStore, CustomizerService]
        });
        service = TestBed.inject(CustomizerService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('getLightDarkColors should mark correct color as active for specific type and background color', () => {
        const backgroundColor = LayoutSidebarBackgroundColor.ManOfSteel;
        const type = LightDarkColorType.Gradient;
        const expectedResult = LIGHT_DARK_COLORS.filter((color: LightDarkColor) => color.type === type).map((color: LightDarkColor) => {
            color.active = color.backgroundColor === backgroundColor;
            return color;
        });

        const result = service.getLightDarkColors(backgroundColor, type);

        expect(result).toEqual(expectedResult);
    });

    it('getTransparentColorsWithShades should mark correct color as active for specific background color', () => {
        const backgroundColor = LayoutSidebarBackgroundColor.BgGlass1;
        const expectedResult = TRANSPARENT_COLORS_WITH_SHADES.map((color: TransparentColor) => {
            color.active = color.backgroundColor === backgroundColor;
            return color;
        });

        const result = service.getTransparentColorsWithShades(backgroundColor);

        expect(result).toEqual(expectedResult);
    });

    it('getTransparentColors should mark correct color as active for specific background color', () => {
        const backgroundColor = LayoutSidebarBackgroundColor.BgGlassHibiscus;
        const expectedResult = TRANSPARENT_COLORS.map((color: TransparentColor) => {
            color.active = color.backgroundColor === backgroundColor;
            return color;
        });

        const result = service.getTransparentColors(backgroundColor);

        expect(result).toEqual(expectedResult);
    });

    it('getLightDarkBgImages should mark correct image as active for specific background image url', () => {
        const backgroundImageUrl = SIDEBAR_BG_1_URL;
        const expectedResult = LIGHT_DARK_BG_IMAGES.map((image: LightDarkImage) => {
            image.active = image.src === backgroundImageUrl;
            return image;
        });

        const result = service.getLightDarkBgImages(backgroundImageUrl);

        expect(result).toEqual(expectedResult);
    });

    describe('changeLayoutVariant should set proper attribues when layout variant is', () => {
        let layoutVariant: LayoutVariant;
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

        it('light', inject([LayoutTemplateStore], (layoutTemplateStore: LayoutTemplateStore) => {
            layoutVariant = LayoutVariant.Light;
            const expectedLayoutTemplate: LayoutTemplate = {
                variant: layoutVariant,
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
            spyOn(layoutTemplateStore, 'updateState');

            service.changeLayoutVariant(layoutTemplate, layoutVariant);

            expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
        }));

        it('dark', inject([LayoutTemplateStore], (layoutTemplateStore: LayoutTemplateStore) => {
            layoutVariant = LayoutVariant.Dark;
            const expectedLayoutTemplate: LayoutTemplate = {
                variant: layoutVariant,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.Black,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_3_URL
                }
            };
            spyOn(layoutTemplateStore, 'updateState');

            service.changeLayoutVariant(layoutTemplate, layoutVariant);

            expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
        }));

        it('transparent', inject([LayoutTemplateStore], (layoutTemplateStore: LayoutTemplateStore) => {
            layoutVariant = LayoutVariant.Transparent;
            const expectedLayoutTemplate: LayoutTemplate = {
                variant: layoutVariant,
                menuPosition: LayoutMenuPosition.Top,
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.BgGlass1,
                    backgroundImage: false,
                    backgroundImageURL: ''
                }
            };
            spyOn(layoutTemplateStore, 'updateState');

            service.changeLayoutVariant(layoutTemplate, layoutVariant);

            expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
        }));
    });

    it('changeLayoutNavbarType should change navbar type', inject([LayoutTemplateStore], (layoutTemplateStore: LayoutTemplateStore) => {
        const layoutNavbarType = LayoutNavbarType.Fixed;
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
            isCustomizerOpen: false,
            navbarType: layoutNavbarType,
            sidebar: {
                collapsed: false,
                size: LayoutSidebarSize.Medium,
                backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                backgroundImage: true,
                backgroundImageURL: SIDEBAR_BG_1_URL
            }
        };
        spyOn(layoutTemplateStore, 'updateState');

        service.changeLayoutNavbarType(layoutTemplate, layoutNavbarType);

        expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
    }));

    it('changeLayoutSidebarColor should change sidebar color', inject([LayoutTemplateStore], (layoutTemplateStore: LayoutTemplateStore) => {
        const color = LIGHT_DARK_COLORS[0];
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
            isCustomizerOpen: false,
            navbarType: LayoutNavbarType.Static,
            sidebar: {
                collapsed: false,
                size: LayoutSidebarSize.Medium,
                backgroundColor: color.backgroundColor,
                backgroundImage: true,
                backgroundImageURL: SIDEBAR_BG_1_URL
            }
        };
        spyOn(layoutTemplateStore, 'updateState');

        service.changeLayoutSidebarColor(layoutTemplate, color);

        expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
    }));

    it('changeLayoutSidebarImage should change sidebar image', inject([LayoutTemplateStore], (layoutTemplateStore: LayoutTemplateStore) => {
        const image = new LightDarkImage(SIDEBAR_BG_4_URL, false);
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
            isCustomizerOpen: false,
            navbarType: LayoutNavbarType.Static,
            sidebar: {
                collapsed: false,
                size: LayoutSidebarSize.Medium,
                backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                backgroundImage: true,
                backgroundImageURL: image.src
            }
        };
        spyOn(layoutTemplateStore, 'updateState');

        service.changeLayoutSidebarImage(layoutTemplate, image);

        expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
    }));

    it('changeLayoutSidebarTransparentColor should change sidebar transparent color', inject(
        [LayoutTemplateStore],
        (layoutTemplateStore: LayoutTemplateStore) => {
            const color = TRANSPARENT_COLORS[0];
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
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: color.backgroundColor,
                    backgroundImage: false,
                    backgroundImageURL: ''
                }
            };
            spyOn(layoutTemplateStore, 'updateState');

            service.changeLayoutSidebarTransparentColor(layoutTemplate, color);

            expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
        }
    ));

    it('displayLayoutSidebarImage should change sidebar transparent color', inject(
        [LayoutTemplateStore],
        (layoutTemplateStore: LayoutTemplateStore) => {
            const event = ({
                target: {
                    checked: false
                } as HTMLInputElement
            } as unknown) as Event;
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
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size: LayoutSidebarSize.Medium,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: false,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };
            spyOn(layoutTemplateStore, 'updateState');

            service.displayLayoutSidebarImage(layoutTemplate, event);

            expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
        }
    ));

    it('changeLayoutSidebarSize should change sidebar transparent color', inject(
        [LayoutTemplateStore],
        (layoutTemplateStore: LayoutTemplateStore) => {
            const size = LayoutSidebarSize.Large;
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
                isCustomizerOpen: false,
                navbarType: LayoutNavbarType.Static,
                sidebar: {
                    collapsed: false,
                    size,
                    backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                    backgroundImage: true,
                    backgroundImageURL: SIDEBAR_BG_1_URL
                }
            };
            spyOn(layoutTemplateStore, 'updateState');

            service.changeLayoutSidebarSize(layoutTemplate, size);

            expect(layoutTemplateStore.updateState).toHaveBeenCalledWith(expectedLayoutTemplate);
        }
    ));
});
