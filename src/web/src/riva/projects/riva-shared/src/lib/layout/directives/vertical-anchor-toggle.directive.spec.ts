import { ChangeDetectorRef } from '@angular/core';
import { inject, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LayoutTemplateStore } from './../stores/layout-template.store';
import { VerticalAnchorToggleDirective } from './vertical-anchor-toggle.directive';
import { VerticalLinkDirective } from './vertical-link.directive';
import { VerticalDirective } from './vertical.directive';

describe('VerticalAnchorToggleDirective', () => {
    let directive: VerticalAnchorToggleDirective;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [RouterTestingModule],
            providers: [VerticalAnchorToggleDirective, VerticalLinkDirective, VerticalDirective, LayoutTemplateStore, ChangeDetectorRef]
        });
        directive = TestBed.inject(VerticalAnchorToggleDirective);
    });

    it('should create an instance', () => {
        expect(directive).toBeTruthy();
    });

    it('onClick should call verticalLinkDirective.toggle', inject(
        [VerticalLinkDirective],
        (verticalLinkDirective: VerticalLinkDirective) => {
            spyOn(verticalLinkDirective, 'toggle').and.callThrough();

            directive.onClick(new MouseEvent('click'));

            expect(verticalLinkDirective.toggle).toHaveBeenCalled();
        }
    ));
});
