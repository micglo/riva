import { Injectable } from '@angular/core';
import { AppConfiguration } from './app-configuration.model';

@Injectable()
export class AppConfigurationService {
    configuration: Partial<AppConfiguration> = {};
}
