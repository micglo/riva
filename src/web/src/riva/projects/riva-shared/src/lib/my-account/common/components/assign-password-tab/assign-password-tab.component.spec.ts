import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AssignPasswordTabComponent } from './assign-password-tab.component';

describe('AssignPasswordTabComponent', () => {
    let component: AssignPasswordTabComponent;
    let fixture: ComponentFixture<AssignPasswordTabComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AssignPasswordTabComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AssignPasswordTabComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
