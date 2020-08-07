import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';
import { LightDarkColorType } from './../enums/light-dark-color-type.enum';

export class LightDarkColor {
    public get backgroundColor(): LayoutSidebarBackgroundColor {
        return this.bgColor;
    }

    public get backgroundClass(): string {
        return this.bgClass;
    }

    public get active(): boolean {
        return this.isActive;
    }
    public set active(value: boolean) {
        this.isActive = value;
    }

    public get type(): LightDarkColorType {
        return this.colorType;
    }

    constructor(
        private bgColor: LayoutSidebarBackgroundColor,
        private bgClass: string,
        private isActive: boolean,
        private colorType: LightDarkColorType
    ) {}
}
