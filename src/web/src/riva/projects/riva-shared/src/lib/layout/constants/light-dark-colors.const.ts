import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';
import { LightDarkColorType } from './../enums/light-dark-color-type.enum';
import { LightDarkColor } from './../models/light-dark-color.model';

export const LIGHT_DARK_COLORS = new Array<LightDarkColor>(
    new LightDarkColor(LayoutSidebarBackgroundColor.Mint, 'gradient-mint', false, LightDarkColorType.Gradient),
    new LightDarkColor(LayoutSidebarBackgroundColor.KingYna, 'gradient-king-yna', false, LightDarkColorType.Gradient),
    new LightDarkColor(LayoutSidebarBackgroundColor.IbizaSunset, 'gradient-ibiza-sunset', false, LightDarkColorType.Gradient),
    new LightDarkColor(LayoutSidebarBackgroundColor.Flickr, 'gradient-flickr', false, LightDarkColorType.Gradient),
    new LightDarkColor(LayoutSidebarBackgroundColor.PurpleBliss, 'gradient-purple-bliss', false, LightDarkColorType.Gradient),
    new LightDarkColor(LayoutSidebarBackgroundColor.ManOfSteel, 'gradient-man-of-steel', false, LightDarkColorType.Gradient),
    new LightDarkColor(LayoutSidebarBackgroundColor.PurpleLove, 'gradient-purple-love', false, LightDarkColorType.Gradient),
    new LightDarkColor(LayoutSidebarBackgroundColor.Black, 'bg-black', false, LightDarkColorType.Solid),
    new LightDarkColor(LayoutSidebarBackgroundColor.White, 'bg-grey', false, LightDarkColorType.Solid),
    new LightDarkColor(LayoutSidebarBackgroundColor.Primary, 'bg-primary', false, LightDarkColorType.Solid),
    new LightDarkColor(LayoutSidebarBackgroundColor.Success, 'bg-success', false, LightDarkColorType.Solid),
    new LightDarkColor(LayoutSidebarBackgroundColor.Warning, 'bg-warning', false, LightDarkColorType.Solid),
    new LightDarkColor(LayoutSidebarBackgroundColor.Info, 'bg-info', false, LightDarkColorType.Solid),
    new LightDarkColor(LayoutSidebarBackgroundColor.Danger, 'bg-danger', false, LightDarkColorType.Solid)
);
