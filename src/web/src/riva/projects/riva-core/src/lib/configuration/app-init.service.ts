import { Injectable } from '@angular/core';
import { from } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AppConfiguration } from './app-configuration.model';
import { AppConfigurationService } from './app-configuration.service';

@Injectable()
export class AppInitService {
    constructor(private appConfigurationService: AppConfigurationService) {}

    public init(): Promise<AppConfiguration> {
        return from(fetch('assets/app-config.json').then(response => response.json()))
            .pipe(tap((config: AppConfiguration) => (this.appConfigurationService.configuration = config)))
            .toPromise();
    }
}
