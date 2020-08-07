import { TestBed } from '@angular/core/testing';
import { WindowModule } from './../window/window.module';
import { StorageModule } from './storage.module';
import { StorageService } from './storage.service';

describe('StorageService', () => {
    let service: StorageService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [StorageModule.forRoot(), WindowModule.forRoot()]
        });
        service = TestBed.inject(StorageService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('getItem should return', () => {
        it('undefined from storage', () => {
            const result = service.getItem<object>('UNDEFINED_ITEM');

            expect(result).toBeUndefined();
        });

        it('item from storage', () => {
            const key = 'STORAGE_SERVICE_GET_ITEM_TEST';
            const item = 'item to test getItem function';
            service.setItem(key, item);

            const result = service.getItem<string>(key);

            expect(result).toBe(item);
        });
    });

    it('setItem should set item in storage', () => {
        const key = 'STORAGE_SERVICE_SET_ITEM_TEST';
        const item = 'item to test setItem function';

        service.setItem(key, item);
        const result = service.getItem<string>(key);

        expect(result).toBe(item);
    });
});
