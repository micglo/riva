import { ChangeDetectorRef } from '@angular/core';
import { async, ComponentFixture, inject, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { SIDEBAR_BG_1_URL } from './../../constants/images.const';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { LayoutTemplate } from './../../models/layout-template.model';
import { LayoutTemplateStore } from './../../stores/layout-template.store';
import { HorizontalMenuComponent } from './horizontal-menu.component';

describe('HorizontalMenuComponent', () => {
    let component: HorizontalMenuComponent;
    let fixture: ComponentFixture<HorizontalMenuComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [HorizontalMenuComponent],
            providers: [LayoutTemplateStore, ChangeDetectorRef]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(HorizontalMenuComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('ngOnInit should set properties', inject([LayoutTemplateStore], (layoutTemplateStore: LayoutTemplateStore) => {
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
        const expectedBackgroundColor = '';
        spyOnProperty(layoutTemplateStore, 'state$', 'get').and.returnValue(of(layoutTemplate));

        component.ngOnInit();

        expect(component.layoutTemplate).toEqual(layoutTemplate);
        expect(component.backgroundColor).toEqual(expectedBackgroundColor);
    }));
});
