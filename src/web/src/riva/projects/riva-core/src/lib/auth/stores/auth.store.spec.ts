import { TestBed } from '@angular/core/testing';
import { AuthStore } from './auth.store';

describe('AuthStore', () => {
    let service: AuthStore;

    beforeEach(() => {
        TestBed.configureTestingModule({ providers: [AuthStore] });
        service = TestBed.inject(AuthStore);
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
