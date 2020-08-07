import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { SIDEBAR_BG_1_URL } from './../constants/images.const';
import { LayoutMenuPosition } from './../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../enums/layout-variant.enum';
import { LayoutServicesModule } from './../layout-services.module';
import { LayoutTemplate } from './../models/layout-template.model';

@Injectable({ providedIn: LayoutServicesModule })
export class LayoutTemplateStore {
    private _subject$: BehaviorSubject<LayoutTemplate>;

    public get state$(): Observable<LayoutTemplate> {
        return this._subject$.asObservable();
    }

    constructor() {
        const layoutTemplate: LayoutTemplate = {
            variant: LayoutVariant.Light,
            menuPosition: LayoutMenuPosition.Top,
            isCustomizerOpen: false,
            navbarType: LayoutNavbarType.Static,
            sidebar: {
                collapsed: false,
                size: LayoutSidebarSize.Medium,
                backgroundColor: LayoutSidebarBackgroundColor.ManOfSteel,
                backgroundImage: true,
                backgroundImageURL: SIDEBAR_BG_1_URL
            }
        };
        this._subject$ = new BehaviorSubject<LayoutTemplate>(layoutTemplate);
    }

    public updateState(layoutTemplate: LayoutTemplate): void {
        this._subject$.next(layoutTemplate);
    }
}
