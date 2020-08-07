import { inject, TestBed } from '@angular/core/testing';
import { FullscreenService } from './../services/fullscreen.service';
import { ToggleFullscreenDirective } from './toggle-fullscreen.directive';

describe('ToggleFullscreenDirective', () => {
    let directive: ToggleFullscreenDirective;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [FullscreenService]
        });
        const fullscreenService = TestBed.inject(FullscreenService);
        directive = new ToggleFullscreenDirective(fullscreenService);
    });

    it('should create an instance', () => {
        expect(directive).toBeTruthy();
    });

    it('onClick should call fullscreenService.toggle', inject([FullscreenService], (fullscreenService: FullscreenService) => {
        spyOn(fullscreenService, 'toggle').and.callThrough();

        directive.onClick();

        expect(fullscreenService.toggle).toHaveBeenCalled();
    }));
});
