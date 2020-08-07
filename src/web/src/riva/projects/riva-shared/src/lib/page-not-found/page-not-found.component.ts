import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'lib-page-not-found',
    templateUrl: './page-not-found.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class PageNotFoundComponent {}
