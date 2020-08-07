import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';

export class TransparentColor {
    public get backgroundColor(): LayoutSidebarBackgroundColor {
        return this.bgColor;
    }

    public get active(): boolean {
        return this.isActive;
    }
    public set active(value: boolean) {
        this.isActive = value;
    }

    constructor(private bgColor: LayoutSidebarBackgroundColor, private isActive: boolean) {}
}
