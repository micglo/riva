import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TranslateModule } from '@ngx-translate/core';
import { AuthCodeRedirectComponent } from './auth-code-redirect.component';

describe('AuthCodeRedirectComponent', () => {
    let component: AuthCodeRedirectComponent;
    let fixture: ComponentFixture<AuthCodeRedirectComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [TranslateModule.forRoot()],
            declarations: [AuthCodeRedirectComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AuthCodeRedirectComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
