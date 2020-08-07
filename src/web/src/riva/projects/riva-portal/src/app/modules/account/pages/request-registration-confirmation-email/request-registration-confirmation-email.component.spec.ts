import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RequestRegistrationConfirmationEmailComponent } from './request-registration-confirmation-email.component';

describe('RequestRegistrationConfirmationEmail', () => {
    let component: RequestRegistrationConfirmationEmailComponent;
    let fixture: ComponentFixture<RequestRegistrationConfirmationEmailComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [RequestRegistrationConfirmationEmailComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(RequestRegistrationConfirmationEmailComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
