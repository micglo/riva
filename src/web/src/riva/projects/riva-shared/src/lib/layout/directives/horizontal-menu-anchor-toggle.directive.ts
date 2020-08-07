import { Directive, HostListener } from '@angular/core';
import { HorizontalMenuLinkDirective } from './horizontal-menu-link.directive';

@Directive({
    selector: '[libHorizontalMenuAnchorToggle]'
})
export class HorizontalMenuAnchorToggleDirective {
    constructor(private horizontalMenuLinkDirective: HorizontalMenuLinkDirective) {}

    @HostListener('mouseenter', ['$event'])
    public onMouseEnter(event: MouseEvent): void {
        this.horizontalMenuLinkDirective.openDropdown();
    }
}
