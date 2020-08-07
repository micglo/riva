import { Renderer2 } from '@angular/core';
import { inject, TestBed } from '@angular/core/testing';
import { LayoutVariant } from './../enums/layout-variant.enum';
import { Renderer2Stub } from './../testing/stubs/renderer2.stub';
import { BodyClassesUpdateService } from './body-classes-update.service';
import { LayoutService } from './layout.service';

describe('LayoutService', () => {
    let service: LayoutService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [{ provide: Renderer2, useClass: Renderer2Stub }, BodyClassesUpdateService, LayoutService]
        });
        service = TestBed.inject(LayoutService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('setLayoutOnVariant should call proper BodyClassesUpdateService function to set layout when layout variant is', () => {
        let layoutVariant: LayoutVariant;
        const bgColor = 'black';

        it('light', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
            layoutVariant = LayoutVariant.Light;
            spyOn(bodyClassesUpdateService, 'updateBodyClassesWhenLayoutVariantIsLight');

            service.setLayoutOnVariant(layoutVariant, bgColor);

            expect(bodyClassesUpdateService.updateBodyClassesWhenLayoutVariantIsLight).toHaveBeenCalled();
        }));

        it('dark', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
            layoutVariant = LayoutVariant.Dark;
            spyOn(bodyClassesUpdateService, 'updateBodyClassesWhenLayoutVariantIsDark').and.callThrough();

            service.setLayoutOnVariant(layoutVariant, bgColor);

            expect(bodyClassesUpdateService.updateBodyClassesWhenLayoutVariantIsDark).toHaveBeenCalled();
        }));

        it('transparent', inject([BodyClassesUpdateService], (bodyClassesUpdateService: BodyClassesUpdateService) => {
            layoutVariant = LayoutVariant.Transparent;
            spyOn(bodyClassesUpdateService, 'updateBodyClassesWhenLayoutVariantIsTransparent').and.callThrough();

            service.setLayoutOnVariant(layoutVariant, bgColor);

            expect(bodyClassesUpdateService.updateBodyClassesWhenLayoutVariantIsTransparent).toHaveBeenCalledWith(bgColor);
        }));
    });
});
