import { TestBed } from '@angular/core/testing';
import { AppConfiguration } from './app-configuration.model';
import { AppConfigurationService } from './app-configuration.service';

describe('AppConfigurationService', () => {
    let service: AppConfigurationService;

    beforeEach(() => {
        TestBed.configureTestingModule({ providers: [AppConfigurationService] });
        service = TestBed.inject(AppConfigurationService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should assign AppConfiguration instance to configuration', () => {
        const appConfiguration: AppConfiguration = {
            api_url: 'api_url',
            auth: {
                issuer: 'issuer',
                client_id: 'client_id',
                redirect_uri: 'redirect_uri',
                silent_refresh_redirect_uri: 'silent_refresh_redirect_uri',
                post_logout_redirect_uri: 'post_logout_redirect_uri',
                scope: 'scope'
            }
        };

        service.configuration = appConfiguration;

        expect(service.configuration).toEqual(appConfiguration);
    });
});
