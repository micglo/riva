import { ChangeDetectorRef } from '@angular/core';
import { inject, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LayoutTemplateStore } from './../stores/layout-template.store';
import { VerticalLinkDirective } from './vertical-link.directive';
import { VerticalDirective } from './vertical.directive';

describe('VerticalLinkDirective', () => {
    let directive: VerticalLinkDirective;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [RouterTestingModule],
            providers: [VerticalLinkDirective, VerticalDirective, LayoutTemplateStore, ChangeDetectorRef]
        });
        directive = TestBed.inject(VerticalLinkDirective);
    });

    it('should create an instance', () => {
        expect(directive).toBeTruthy();
    });

    it('ngOnInit should call verticalDirective.addLink', inject([VerticalDirective], (verticalDirective: VerticalDirective) => {
        spyOn(verticalDirective, 'addLink').and.callThrough();

        directive.ngOnInit();

        expect(verticalDirective.addLink).toHaveBeenCalledWith(directive);
    }));

    it('toggle should change open value and call verticalDirective.closeOtherLinks', inject(
        [VerticalDirective],
        (verticalDirective: VerticalDirective) => {
            directive.open = false;
            spyOn(verticalDirective, 'closeOtherLinks').and.callThrough();

            directive.toggle();

            expect(directive.open).toEqual(true);
            expect(verticalDirective.closeOtherLinks).toHaveBeenCalledWith(directive);
        }
    ));
});
