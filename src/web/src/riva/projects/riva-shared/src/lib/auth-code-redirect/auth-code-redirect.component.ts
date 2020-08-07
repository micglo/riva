import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'lib-auth-code-redirect',
    template: './auth-code-redirect.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthCodeRedirectComponent {}
