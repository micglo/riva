import { HorizontalMenuLinkDirective } from './horizontal-menu-link.directive';
import { HorizontalMenuDirective } from './horizontal-menu.directive';

describe('HorizontalMenuDirective', () => {
    // let directive: HorizontalMenuDirective;

    // beforeAll(() => {
    //     directive = new HorizontalMenuDirective();
    // });

    it('should create an instance', () => {
        const directive = new HorizontalMenuDirective();
        expect(directive).toBeTruthy();
    });

    it('closeOtherLinks should set other links show to false', () => {
        const directive = new HorizontalMenuDirective();
        const firstLink = new HorizontalMenuLinkDirective(directive);
        directive.addLink(firstLink);
        firstLink.level = 1;
        firstLink.show = true;
        const secondLink = new HorizontalMenuLinkDirective(directive);
        directive.addLink(secondLink);
        secondLink.level = 1;

        secondLink.show = true;

        expect(firstLink.show).toEqual(false);
        expect(secondLink.show).toEqual(true);
    });

    it('onMouseLeave should set all links show to false', () => {
        const directive = new HorizontalMenuDirective();
        const link = new HorizontalMenuLinkDirective(directive);
        directive.addLink(link);
        link.show = true;

        directive.onMouseLeave(new MouseEvent('click'));

        expect(link.show).toEqual(false);
    });
});
