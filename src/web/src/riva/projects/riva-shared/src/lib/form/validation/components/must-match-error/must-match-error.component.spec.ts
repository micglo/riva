import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MustMatchErrorComponent } from './must-match-error.component';

describe('MustMatchErrorComponent', () => {
    let component: MustMatchErrorComponent;
    let fixture: ComponentFixture<MustMatchErrorComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [MustMatchErrorComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(MustMatchErrorComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
