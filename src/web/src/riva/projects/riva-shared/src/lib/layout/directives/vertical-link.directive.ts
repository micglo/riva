import { Directive, HostBinding, Input, OnInit } from '@angular/core';
import { VerticalDirective } from './vertical.directive';

@Directive({
    selector: '[libVerticalLink]'
})
export class VerticalLinkDirective implements OnInit {
    @Input()
    public parent: string;

    @Input()
    public level: number;

    @Input()
    public hasSub: boolean;

    @Input()
    public path: string;

    @HostBinding('class.open')
    @Input()
    public get open(): boolean {
        return this._open;
    }
    public set open(value: boolean) {
        this._open = value;
    }

    private _open: boolean;

    public constructor(private verticalDirective: VerticalDirective) {}

    public ngOnInit(): void {
        this.verticalDirective.addLink(this);
    }

    public toggle(): void {
        this._open = !this._open;
        if (this._open) {
            this.verticalDirective.closeOtherLinks(this);
        }
    }
}
