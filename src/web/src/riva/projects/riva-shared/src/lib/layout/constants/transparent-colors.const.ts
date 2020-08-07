import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';
import { TransparentColor } from './../models/transparent-color.model';

export const TRANSPARENT_COLORS = new Array<TransparentColor>(
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlassHibiscus, false),
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlassPurplePizzazz, false),
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlassBlueLagoon, false),
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlassElectricViolet, false),
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlassProtage, false),
    new TransparentColor(LayoutSidebarBackgroundColor.BgGlassTundora, false)
);
