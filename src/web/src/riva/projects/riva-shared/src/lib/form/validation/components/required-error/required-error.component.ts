import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'lib-required-error',
    templateUrl: './required-error.component.html'
})
export class RequiredErrorComponent {
    @Input() public field: FormControl;
}
