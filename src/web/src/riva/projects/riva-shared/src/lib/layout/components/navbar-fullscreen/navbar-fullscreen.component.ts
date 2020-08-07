import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'lib-navbar-fullscreen',
    templateUrl: './navbar-fullscreen.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavbarFullscreenComponent {
    private maximizeClass = 'ft-maximize';
    private minimizeClass = 'ft-minimize';
    private _fullscreenClass = this.maximizeClass;

    public get fullscreenClass(): string {
        return this._fullscreenClass;
    }

    public toggleFullscreenClass(): void {
        this._fullscreenClass = this._fullscreenClass === this.maximizeClass ? this.minimizeClass : this.maximizeClass;
    }
}
