import { TestBed } from '@angular/core/testing';
import { ShowSmallScreenMenuStore } from './show-small-screen-manu.store';

describe('ShowSmallScreenMenuStore', () => {
    let service: ShowSmallScreenMenuStore;

    beforeEach(() => {
        TestBed.configureTestingModule({ providers: [ShowSmallScreenMenuStore] });
        service = TestBed.inject(ShowSmallScreenMenuStore);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('updateState should change state to true', () => {
        const expectedState = true;

        service.updateState(true);
        service.state$.subscribe((state: boolean) => expect(state).toEqual(expectedState));
    });
});
