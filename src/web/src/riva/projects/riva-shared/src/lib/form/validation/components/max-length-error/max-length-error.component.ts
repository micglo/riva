import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'lib-max-length-error',
    templateUrl: './max-length-error.component.html'
})
export class MaxLengthErrorComponent {
    @Input() public field: FormControl;
    @Input() public maxLength: number;
}
