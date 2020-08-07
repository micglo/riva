import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'lib-must-match-error',
    templateUrl: './must-match-error.component.html'
})
export class MustMatchErrorComponent {
    @Input() public field: FormControl;
    @Input() public matchingFieldName: string;
}
