import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'lib-min-length-error',
    templateUrl: './min-length-error.component.html'
})
export class MinLengthErrorComponent {
    @Input() public field: FormControl;
    @Input() public minLength: number;
}
