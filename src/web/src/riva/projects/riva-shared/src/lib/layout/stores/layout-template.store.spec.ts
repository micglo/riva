import { TestBed } from '@angular/core/testing';
import { LayoutMenuPosition } from './../enums/layout-menu-position.enum';
import { LayoutNavbarType } from './../enums/layout-navbar-type.enum';
import { LayoutSidebarBackgroundColor } from './../enums/layout-sidebar-background-color.enum';
import { LayoutSidebarSize } from './../enums/layout-sidebar-size.enum';
import { LayoutVariant } from './../enums/layout-variant.enum';
import { LayoutTemplate } from './../models/layout-template.model';
import { LayoutTemplateStore } from './layout-template.store';

describe('LayoutTemplateStore', () => {
    let service: LayoutTemplateStore;

    beforeEach(() => {
        TestBed.configureTestingModule({ providers: [LayoutTemplateStore] });
        service = TestBed.inject(LayoutTemplateStore);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('updateLayoutTemplate should update LayoutTemplate', () => {
        const layoutTemplateToUpdate = {
            variant: LayoutVariant.Dark,
            menuPosition: LayoutMenuPosition.Top,
            isCustomizerOpen: false,
            navbarType: LayoutNavbarType.Fixed,
            sidebar: {
                collapsed: true,
                size: LayoutSidebarSize.Small,
                backgroundColor: LayoutSidebarBackgroundColor.BgGlass1,
                backgroundImage: false,
                backgroundImageURL: 'assets/img/sidebar-bg/02.jpg'
            }
        };

        service.updateState(layoutTemplateToUpdate);

        service.state$.subscribe((state: LayoutTemplate) => expect(state).toEqual(layoutTemplateToUpdate));
    });
});
