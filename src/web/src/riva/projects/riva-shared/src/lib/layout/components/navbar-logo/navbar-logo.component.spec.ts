import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DARK_LOGO_URL, LIGHT_LOGO_URL } from './../../constants/images.const';
import { LayoutMenuPosition } from './../../enums/layout-menu-position.enum';
import { LayoutVariant } from './../../enums/layout-variant.enum';
import { NavbarLogoComponent } from './navbar-logo.component';

describe('NavbarLogoComponent', () => {
    let component: NavbarLogoComponent;
    let fixture: ComponentFixture<NavbarLogoComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [NavbarLogoComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(NavbarLogoComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('logoUrl should return', () => {
        it('dark logo url when layout variant is light', () => {
            component.layoutVariant = LayoutVariant.Light;
            const expectedLogoUrl = DARK_LOGO_URL;

            expect(component.logoUrl).toBe(expectedLogoUrl);
        });

        it('ligh logo url when layout variant is dark', () => {
            component.layoutVariant = LayoutVariant.Dark;
            const expectedLogoUrl = LIGHT_LOGO_URL;

            expect(component.logoUrl).toBe(expectedLogoUrl);
        });
    });

    it('should set menuPosition', () => {
        const menuPosition = LayoutMenuPosition.Top;
        component.menuPosition = menuPosition;

        expect(component.menuPosition).toBe(menuPosition);
    });
});
