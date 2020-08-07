import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RequestPasswordResetEmailComponent } from './request-password-reset-email.component';

describe('RequestPasswordResetEmailComponent', () => {
    let component: RequestPasswordResetEmailComponent;
    let fixture: ComponentFixture<RequestPasswordResetEmailComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [RequestPasswordResetEmailComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(RequestPasswordResetEmailComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
