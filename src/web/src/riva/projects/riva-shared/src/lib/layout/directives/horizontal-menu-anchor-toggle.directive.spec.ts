import { HorizontalMenuAnchorToggleDirective } from './horizontal-menu-anchor-toggle.directive';
import { HorizontalMenuLinkDirective } from './horizontal-menu-link.directive';
import { HorizontalMenuDirective } from './horizontal-menu.directive';

describe('HorizontalMenuAnchorToggleDirective', () => {
    const horizontalMenuLinkDirective = new HorizontalMenuLinkDirective(new HorizontalMenuDirective());
    let directive: HorizontalMenuAnchorToggleDirective;

    beforeAll(() => {
        directive = new HorizontalMenuAnchorToggleDirective(horizontalMenuLinkDirective);
    });

    it('should create an instance', () => {
        expect(directive).toBeTruthy();
    });

    it('onMouseEnter should call horizontalMenuLinkDirective.openDropdown', () => {
        spyOn(horizontalMenuLinkDirective, 'openDropdown').and.callThrough();

        directive.onMouseEnter(new MouseEvent('click'));

        expect(horizontalMenuLinkDirective.openDropdown).toHaveBeenCalled();
    });
});
