import { TestBed } from '@angular/core/testing';
import { FullscreenService } from './fullscreen.service';

describe('SmallScreenMenuService', () => {
    let service: FullscreenService;

    beforeEach(() => {
        TestBed.configureTestingModule({ providers: [FullscreenService] });
        service = TestBed.inject(FullscreenService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
