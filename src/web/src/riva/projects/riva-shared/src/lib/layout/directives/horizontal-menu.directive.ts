import { Directive, HostListener } from '@angular/core';
import { HorizontalMenuLinkDirective } from './horizontal-menu-link.directive';

@Directive({
    selector: '[libHorizontalMenu]'
})
export class HorizontalMenuDirective {
    private _links = new Array<HorizontalMenuLinkDirective>();

    public addLink(link: HorizontalMenuLinkDirective): void {
        this._links.push(link);
    }

    public closeOtherLinks(openLink: HorizontalMenuLinkDirective): void {
        this._links.forEach((link: HorizontalMenuLinkDirective) => {
            if (link !== openLink && (openLink.level.toString() === '1' || link.level === openLink.level)) {
                link.show = false;
            }
        });
    }

    @HostListener('mouseleave', ['$event'])
    public onMouseLeave(event: MouseEvent): void {
        this._links.forEach((link: HorizontalMenuLinkDirective) => {
            link.show = false;
        });
    }
}
