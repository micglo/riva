import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponseHeader, AppConfigurationService } from 'riva-core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountServicesModule } from './../../account-services.module';
import { CreateNewAccount } from './../models/create-new-account.model';
import { RegistrationConfirmation } from './../models/registration-confirmation.model';
import { RequestPasswordResetEmail } from './../models/request-password-reset-email.model';
import { RequestRegistrationConfirmationEmail } from './../models/request-registration-confirmation-email.model';
import { ResetPassword } from './../models/reset-password.model';

@Injectable({ providedIn: AccountServicesModule })
export class AccountProxy {
    private _endpointUrl: string;
    private _apiVersion = '1.0';

    constructor(appConfigurationService: AppConfigurationService, private httpClient: HttpClient) {
        this._endpointUrl = `${appConfigurationService.configuration.api_url}/api/accounts`;
    }

    public createAccount(createNewAccount: CreateNewAccount): Observable<string> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        return (
            this.httpClient
                .post(`${this._endpointUrl}`, createNewAccount, { headers, observe: 'response' })
                // tslint:disable-next-line: ban-types
                .pipe(map((resp: HttpResponse<Object>) => resp.headers.get(ApiResponseHeader.XCorrelationId)))
        );
    }

    // tslint:disable-next-line: ban-types
    public confirmAccount(registrationConfirmation: RegistrationConfirmation): Observable<Object> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        const options = { headers };
        return this.httpClient.post(`${this._endpointUrl}/confirmations`, registrationConfirmation, options);
    }

    // tslint:disable-next-line: ban-types
    public requestAccountConfirmationToken(requestRegistrationConfirmationEmail: RequestRegistrationConfirmationEmail): Observable<Object> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        const options = { headers };
        return this.httpClient.post(`${this._endpointUrl}/confirmations/tokens`, requestRegistrationConfirmationEmail, options);
    }

    // tslint:disable-next-line: ban-types
    public requestPasswordResetToken(requestPasswordResetEmail: RequestPasswordResetEmail): Observable<Object> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        const options = { headers };
        return this.httpClient.post(`${this._endpointUrl}/passwords/tokens`, requestPasswordResetEmail, options);
    }

    // tslint:disable-next-line: ban-types
    public resetPassword(resetPassword: ResetPassword): Observable<Object> {
        const headers = new HttpHeaders().set('api-version', this._apiVersion);
        const options = { headers };
        return this.httpClient.post(`${this._endpointUrl}/passwords/resets`, resetPassword, options);
    }
}
