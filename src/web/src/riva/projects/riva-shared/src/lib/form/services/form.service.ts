import { Injectable } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { FormServicesModule } from './../form-services.module';

@Injectable({ providedIn: FormServicesModule })
export class FormService {
    public isControlTouchedAndDirty(abstractControl: AbstractControl): boolean {
        return abstractControl.touched && abstractControl.dirty;
    }
}
