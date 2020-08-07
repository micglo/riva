import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MinValueErrorComponent } from './min-value-error.component';

describe('MinValueErrorComponent', () => {
    let component: MinValueErrorComponent;
    let fixture: ComponentFixture<MinValueErrorComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [MinValueErrorComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(MinValueErrorComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
