import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaxLengthErrorComponent } from './max-length-error.component';

describe('MaxLengthErrorComponent', () => {
    let component: MaxLengthErrorComponent;
    let fixture: ComponentFixture<MaxLengthErrorComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [MaxLengthErrorComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(MaxLengthErrorComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
