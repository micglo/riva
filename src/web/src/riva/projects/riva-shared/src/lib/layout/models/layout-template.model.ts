import { LayoutMenuPosition } from './../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../enums/layout-variant.enum';

export interface LayoutTemplate {
    variant: LayoutVariant;
    menuPosition: LayoutMenuPosition;
    isCustomizerOpen: boolean;
    navbarType: LayoutNavbarType;
    sidebar: LayoutSidebar;
}

export interface LayoutSidebar {
    collapsed: boolean;
    size: LayoutSidebarSize;
    backgroundColor: LayoutSidebarBackgroundColor;
    backgroundImage: boolean;
    backgroundImageURL: string;
}
