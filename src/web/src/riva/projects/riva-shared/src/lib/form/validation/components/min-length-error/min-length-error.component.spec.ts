import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MinLengthErrorComponent } from './min-length-error.component';

describe('MinLengthErrorComponent', () => {
    let component: MinLengthErrorComponent;
    let fixture: ComponentFixture<MinLengthErrorComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [MinLengthErrorComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(MinLengthErrorComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
