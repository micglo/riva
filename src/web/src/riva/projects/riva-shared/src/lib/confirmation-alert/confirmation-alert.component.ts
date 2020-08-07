import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { Icon } from './icon.enum';

@Component({
    selector: 'lib-confirmation-alert',
    templateUrl: './confirmation-alert.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ConfirmationAlertComponent {
    private _confirmButtonClass: string;
    private _cancelButtonClass: string;

    @Input() public titleKey: string;
    @Input() public textKey: string;
    @Input() public confirmButtonTextKey: string;
    @Input() public icon: Icon;
    @Input() public set confirmButtonClass(value: string) {
        this._confirmButtonClass = value;
    }
    @Input() public set cancelButtonClass(value: string) {
        this._cancelButtonClass = value;
    }
    @Output() public confirmed = new EventEmitter<boolean>();
    @ViewChild('confirmationSwal') public confirmationSwal: SwalComponent;

    public get customClass(): { confirmButton: string; cancelButton: string } {
        return {
            confirmButton: this._confirmButtonClass,
            cancelButton: this._cancelButtonClass
        };
    }

    public fire(): void {
        this.confirmationSwal.fire();
    }

    public onConfirm(): void {
        this.confirmed.emit(true);
    }

    public onCancel(): void {
        this.confirmed.emit(false);
    }
}
