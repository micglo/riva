import { TestBed } from '@angular/core/testing';
import { AppConfigurationService } from './app-configuration.service';
import { AppInitService } from './app-init.service';

describe('AppInitService', () => {
    let service: AppInitService;

    beforeEach(() => {
        TestBed.configureTestingModule({ providers: [AppConfigurationService, AppInitService] });
        service = TestBed.inject(AppInitService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
