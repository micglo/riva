import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';
import { TransparentColor } from './../models/transparent-color.model';

export const TRANSPARENT_COLORS_WITH_SHADES = new Array<TransparentColor>(
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlass1, false),
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlass2, false),
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlass3, false),
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlass4, false)
);
