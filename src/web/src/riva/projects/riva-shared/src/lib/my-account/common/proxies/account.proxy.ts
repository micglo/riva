import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponseHeader, AppConfigurationService } from 'riva-core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MyAccountServicesModule } from './../../my-account-services.module';
import { AssignPassword } from './../models/assign-password.model';
import { ChangePassword } from './../models/change-password.model';
import { MyAccount } from './../models/my-account.model';

@Injectable({ providedIn: MyAccountServicesModule })
export class AccountProxy {
    private _endpointUrl: string;
    private _apiVersion = '1.0';

    constructor(appConfigurationService: AppConfigurationService, private httpClient: HttpClient) {
        this._endpointUrl = `${appConfigurationService.configuration.api_url}/api/accounts`;
    }

    public getById(id: string): Observable<MyAccount> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        const options = { headers };
        return this.httpClient.get<MyAccount>(`${this._endpointUrl}/${id}`, options);
    }

    // tslint:disable-next-line: ban-types
    public assignPassword(id: string, assignPassword: AssignPassword): Observable<Object> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        const options = { headers };
        return this.httpClient.post(`${this._endpointUrl}/${id}/passwords/assignments`, assignPassword, options);
    }

    // tslint:disable-next-line: ban-types
    public changePassword(id: string, changePassword: ChangePassword): Observable<Object> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        const options = { headers };
        return this.httpClient.post(`${this._endpointUrl}/${id}/passwords/changes`, changePassword, options);
    }

    public delete(id: string): Observable<string> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        return (
            this.httpClient
                .delete(`${this._endpointUrl}/${id}`, { headers, observe: 'response' })
                // tslint:disable-next-line: ban-types
                .pipe(map((resp: HttpResponse<Object>) => resp.headers.get(ApiResponseHeader.XCorrelationId)))
        );
    }
}
