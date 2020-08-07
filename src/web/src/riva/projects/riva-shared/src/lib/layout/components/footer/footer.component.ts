import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'lib-footer',
    templateUrl: './footer.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class FooterComponent {
    private _currentDate = new Date();

    public get currentDate(): Date {
        return this._currentDate;
    }
}
