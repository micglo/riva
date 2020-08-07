import {
    LayoutMenuPosition,
    LayoutNavbarType,
    LayoutSidebarBackgroundColor,
    LayoutSidebarSize,
    LayoutTemplate,
    LayoutVariant,
    SIDEBAR_BG_1_URL
} from 'riva-shared';

export const LAYOUT_TEMPLATE: LayoutTemplate = {
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
