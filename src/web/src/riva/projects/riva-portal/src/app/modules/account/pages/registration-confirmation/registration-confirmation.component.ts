import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { SpinnerService } from 'riva-core';
import { filter, map, switchMap, tap } from 'rxjs/operators';
import { RegistrationConfirmation } from './../../common/models/registration-confirmation.model';
import { AccountService } from './../../common/services/account.service';
import { RegistrationConfirmationService } from './../../common/services/registration-confirmation.service';

@Component({
    selector: 'app-registration-confirmation',
    templateUrl: './registration-confirmation.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [RegistrationConfirmationService]
})
export class RegistrationConfirmationComponent implements OnInit {
    constructor(
        private route: ActivatedRoute,
        private registrationConfirmationService: RegistrationConfirmationService,
        private accountService: AccountService,
        private spinnerService: SpinnerService
    ) {}

    public ngOnInit(): void {
        this.route.queryParams
            .pipe(
                filter((params: Params) => params.email && params.code),
                map((params: Params) => {
                    return {
                        email: params.email,
                        code: params.code
                    } as RegistrationConfirmation;
                }),
                tap(() => this.spinnerService.show()),
                switchMap((registrationConfirmation: RegistrationConfirmation) =>
                    this.accountService.confirmAccount(registrationConfirmation)
                )
            )
            .subscribe(
                () => {
                    this.spinnerService.hide();
                    this.registrationConfirmationService.displaySuccessMessageAndRedirectToHome();
                },
                (error: HttpErrorResponse) => {
                    this.spinnerService.hide();
                    this.registrationConfirmationService.disaplyErrorMessageAndRedirectToHome(error);
                }
            );
    }
}
