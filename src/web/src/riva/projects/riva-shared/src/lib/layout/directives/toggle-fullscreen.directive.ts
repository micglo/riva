import { Directive, HostListener } from '@angular/core';
import { FullscreenService } from './../services/fullscreen.service';

@Directive({
    selector: '[libToggleFullscreen]'
})
export class ToggleFullscreenDirective {
    constructor(private fullscreenService: FullscreenService) {}

    @HostListener('click')
    public onClick(): void {
        this.fullscreenService.toggle();
    }
}
