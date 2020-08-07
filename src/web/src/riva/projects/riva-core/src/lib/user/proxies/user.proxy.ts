import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiResponseHeader } from './../../api/api-response-header.enum';
import { AppConfigurationService } from './../../configuration/app-configuration.service';
import { UpdateUser } from './../models/update-user.model';
import { User } from './../models/user.model';

@Injectable()
export class UserProxy {
    private _endpointUrl: string;
    private _apiVersion = '1.0';

    constructor(appConfigurationService: AppConfigurationService, private httpClient: HttpClient) {
        this._endpointUrl = `${appConfigurationService.configuration.api_url}/api/users`;
    }

    getById(id: string): Observable<User> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        const options = { headers };
        return this.httpClient.get<User>(`${this._endpointUrl}/${id}`, options);
    }

    update(updateUser: UpdateUser): Observable<string> {
        const formData = this.createUpdateUserFormData(updateUser);
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        return (
            this.httpClient
                .put(`${this._endpointUrl}/${updateUser.id}`, formData, { headers, observe: 'response' })
                // tslint:disable-next-line: ban-types
                .pipe(map((resp: HttpResponse<Object>) => resp.headers.get(ApiResponseHeader.XCorrelationId)))
        );
    }

    private createUpdateUserFormData(updateUser: UpdateUser): FormData {
        const formData = new FormData();
        formData.append('id', updateUser.id);
        formData.append('serviceActive', updateUser.serviceActive.toString());
        formData.append('announcementPreferenceLimit', updateUser.announcementPreferenceLimit.toString());
        formData.append('announcementSendingFrequency', updateUser.announcementSendingFrequency);
        formData.append('picture', updateUser.picture);
        return formData;
    }
}
