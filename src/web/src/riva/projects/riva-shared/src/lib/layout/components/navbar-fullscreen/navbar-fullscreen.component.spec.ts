import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NavbarFullscreenComponent } from './navbar-fullscreen.component';

describe('NavbarFullscreenComponent', () => {
    let component: NavbarFullscreenComponent;
    let fixture: ComponentFixture<NavbarFullscreenComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [NavbarFullscreenComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(NavbarFullscreenComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('fullscreenClass should return maximize class', () => {
        const maximizeClass = 'ft-maximize';

        expect(component.fullscreenClass).toBe(maximizeClass);
    });

    it('toggleFullscreenClass should change fullscreenClass', () => {
        const expectedClass = 'ft-minimize';

        component.toggleFullscreenClass();

        expect(component.fullscreenClass).toBe(expectedClass);
    });
});
