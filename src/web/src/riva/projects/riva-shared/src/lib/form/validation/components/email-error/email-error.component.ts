import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'lib-email-error',
    templateUrl: './email-error.component.html'
})
export class EmailErrorComponent {
    @Input() public field: FormControl;
}
