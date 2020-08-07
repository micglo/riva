import { Injectable } from '@angular/core';
import { UserDeleteCorrelationIdStore } from 'riva-core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MyAccountServicesModule } from './../../my-account-services.module';
import { AssignPassword } from './../models/assign-password.model';
import { ChangePassword } from './../models/change-password.model';
import { MyAccount } from './../models/my-account.model';
import { AccountProxy } from './../proxies/account.proxy';

@Injectable({ providedIn: MyAccountServicesModule })
export class AccountService {
    constructor(private accountProxy: AccountProxy, private userDeleteCorrelationIdStore: UserDeleteCorrelationIdStore) {}

    public getById(id: string): Observable<MyAccount> {
        return this.accountProxy.getById(id);
    }

    // tslint:disable-next-line: ban-types
    public assignPassword(id: string, assignPassword: AssignPassword): Observable<Object> {
        return this.accountProxy.assignPassword(id, assignPassword);
    }

    // tslint:disable-next-line: ban-types
    public changePassword(id: string, changePassword: ChangePassword): Observable<Object> {
        return this.accountProxy.changePassword(id, changePassword);
    }

    public delete(id: string): Observable<string> {
        return this.accountProxy
            .delete(id)
            .pipe(tap((correlationId: string) => this.userDeleteCorrelationIdStore.addCorrelationId(correlationId)));
    }
}
