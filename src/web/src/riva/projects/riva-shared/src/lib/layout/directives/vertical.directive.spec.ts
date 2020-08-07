import { ChangeDetectorRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LayoutTemplateStore } from './../stores/layout-template.store';
import { VerticalDirective } from './vertical.directive';

describe('VerticalDirective', () => {
    let directive: VerticalDirective;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [RouterTestingModule],
            providers: [VerticalDirective, LayoutTemplateStore, ChangeDetectorRef]
        });
        directive = TestBed.inject(VerticalDirective);
    });

    it('should create an instance', () => {
        expect(directive).toBeTruthy();
    });
});
