import { HorizontalMenuLinkDirective } from './horizontal-menu-link.directive';
import { HorizontalMenuDirective } from './horizontal-menu.directive';

describe('HorizontalMenuLinkDirective', () => {
    const horizontalMenuDirective = new HorizontalMenuDirective();
    let directive: HorizontalMenuLinkDirective;

    beforeAll(() => {
        directive = new HorizontalMenuLinkDirective(horizontalMenuDirective);
    });

    it('should create an instance', () => {
        expect(directive).toBeTruthy();
    });

    it('ngOnInit should call horizontalMenuDirective.addLink', () => {
        spyOn(horizontalMenuDirective, 'addLink').and.callThrough();

        directive.ngOnInit();

        expect(horizontalMenuDirective.addLink).toHaveBeenCalledWith(directive);
    });

    it('openDropdown should set show to true and call horizontalMenuDirective.addLink', () => {
        directive.show = false;
        spyOn(horizontalMenuDirective, 'closeOtherLinks').and.callThrough();

        directive.openDropdown();

        expect(directive.show).toEqual(true);
        expect(horizontalMenuDirective.closeOtherLinks).toHaveBeenCalledWith(directive);
    });
});
