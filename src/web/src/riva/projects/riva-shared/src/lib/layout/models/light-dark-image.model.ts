export class LightDarkImage {
    public get src(): string {
        return this.imgSrc;
    }

    public get active(): boolean {
        return this.isActive;
    }
    public set active(value: boolean) {
        this.isActive = value;
    }

    constructor(private imgSrc: string, private isActive: boolean) {}
}
