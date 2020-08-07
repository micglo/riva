import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'lib-min-value-error',
    templateUrl: './min-value-error.component.html'
})
export class MinValueErrorComponent {
    @Input() public field: FormControl;
    @Input() public minValue: number;
}
