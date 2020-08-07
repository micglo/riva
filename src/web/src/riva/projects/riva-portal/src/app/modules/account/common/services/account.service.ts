import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AccountRegistrationCorrelationIdStore } from './../../../../core/stores/account-registration-correlation-id.store';
import { AccountServicesModule } from './../../account-services.module';
import { CreateNewAccount } from './../models/create-new-account.model';
import { RegistrationConfirmation } from './../models/registration-confirmation.model';
import { RequestPasswordResetEmail } from './../models/request-password-reset-email.model';
import { RequestRegistrationConfirmationEmail } from './../models/request-registration-confirmation-email.model';
import { ResetPassword } from './../models/reset-password.model';
import { AccountProxy } from './../proxies/account.proxy';

@Injectable({ providedIn: AccountServicesModule })
export class AccountService {
    constructor(private accountProxy: AccountProxy, private accountRegistrationCorrelationIdStore: AccountRegistrationCorrelationIdStore) {}

    public createAccount(createNewAccount: CreateNewAccount): Observable<string> {
        return this.accountProxy
            .createAccount(createNewAccount)
            .pipe(tap((correlationId: string) => this.accountRegistrationCorrelationIdStore.addCorrelationId(correlationId)));
    }

    // tslint:disable-next-line: ban-types
    public confirmAccount(registrationConfirmation: RegistrationConfirmation): Observable<Object> {
        return this.accountProxy.confirmAccount(registrationConfirmation);
    }

    // tslint:disable-next-line: ban-types
    public requestAccountConfirmationToken(requestRegistrationConfirmationEmail: RequestRegistrationConfirmationEmail): Observable<Object> {
        return this.accountProxy.requestAccountConfirmationToken(requestRegistrationConfirmationEmail);
    }

    // tslint:disable-next-line: ban-types
    public requestPasswordResetToken(requestPasswordResetEmail: RequestPasswordResetEmail): Observable<Object> {
        return this.accountProxy.requestPasswordResetToken(requestPasswordResetEmail);
    }

    // tslint:disable-next-line: ban-types
    public resetPassword(resetPassword: ResetPassword): Observable<Object> {
        return this.accountProxy.resetPassword(resetPassword);
    }
}
