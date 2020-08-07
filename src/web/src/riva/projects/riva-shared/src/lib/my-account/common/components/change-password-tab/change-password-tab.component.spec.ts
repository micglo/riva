import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangePasswordTabComponent } from './change-password-tab.component';

describe('ChangePasswordTabComponent', () => {
    let component: ChangePasswordTabComponent;
    let fixture: ComponentFixture<ChangePasswordTabComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ChangePasswordTabComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ChangePasswordTabComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
