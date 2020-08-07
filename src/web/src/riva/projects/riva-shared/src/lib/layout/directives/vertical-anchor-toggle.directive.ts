import { Directive, HostListener } from '@angular/core';
import { VerticalLinkDirective } from './vertical-link.directive';

@Directive({
    selector: '[libVerticalAnchorToggle]'
})
export class VerticalAnchorToggleDirective {
    constructor(private verticalLinkDirective: VerticalLinkDirective) {}

    @HostListener('click', ['$event'])
    public onClick(event: MouseEvent): void {
        this.verticalLinkDirective.toggle();
    }
}
