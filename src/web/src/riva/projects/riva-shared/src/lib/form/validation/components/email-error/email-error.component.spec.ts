import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { EmailErrorComponent } from './email-error.component';

describe('EmailErrorComponent', () => {
    let component: EmailErrorComponent;
    let fixture: ComponentFixture<EmailErrorComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [EmailErrorComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(EmailErrorComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
