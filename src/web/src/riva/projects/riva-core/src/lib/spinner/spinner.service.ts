import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class SpinnerService {
    constructor(private ngxSpinnerService: NgxSpinnerService) {}

    public show(): void {
        this.ngxSpinnerService.show();
    }

    public hide(): void {
        this.ngxSpinnerService.hide();
    }
}
