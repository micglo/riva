import { Injectable } from '@angular/core';
import { LayoutVariant } from './../enums/layout-variant.enum';
import { BodyClassesUpdateService } from './body-classes-update.service';

@Injectable()
export class LayoutService {
    constructor(private bodyClassesUpdateService: BodyClassesUpdateService) {}

    public setLayoutOnVariant(variant: LayoutVariant, bgColor: string): void {
        switch (variant) {
            case LayoutVariant.Light: {
                this.bodyClassesUpdateService.updateBodyClassesWhenLayoutVariantIsLight();
                break;
            }
            case LayoutVariant.Dark: {
                this.bodyClassesUpdateService.updateBodyClassesWhenLayoutVariantIsDark();
                break;
            }
            default: {
                this.bodyClassesUpdateService.updateBodyClassesWhenLayoutVariantIsTransparent(bgColor);
                break;
            }
        }
    }
}
