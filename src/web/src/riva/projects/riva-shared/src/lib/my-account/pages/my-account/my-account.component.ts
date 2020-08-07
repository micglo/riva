import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'lib-my-account',
    templateUrl: './my-account.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyAccountComponent {}
