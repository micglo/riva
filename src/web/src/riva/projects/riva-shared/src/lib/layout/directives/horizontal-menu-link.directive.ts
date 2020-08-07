import { Directive, HostBinding, Input, OnInit } from '@angular/core';
import { HorizontalMenuDirective } from './horizontal-menu.directive';

@Directive({
    selector: '[libHorizontalMenuLink]'
})
export class HorizontalMenuLinkDirective implements OnInit {
    private _show: boolean;
    private _level: number;

    @Input()
    public get level(): number {
        return this._level;
    }
    public set level(value: number) {
        this._level = value;
    }

    @HostBinding('class.show')
    public get show(): boolean {
        return this._show;
    }
    public set show(value: boolean) {
        this._show = value;
        if (value) {
            this.horizontalMenuDirective.closeOtherLinks(this);
        }
    }

    public constructor(private horizontalMenuDirective: HorizontalMenuDirective) {}

    public ngOnInit(): void {
        this.horizontalMenuDirective.addLink(this);
    }

    public openDropdown(): void {
        this.show = true;
    }
}
