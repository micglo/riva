import { ChangeDetectorRef, Renderer2 } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { BodyClassesUpdateService } from './../../services/body-classes-update.service';
import { LayoutService } from './../../services/layout.service';
import { LayoutTemplateStore } from './../../stores/layout-template.store';
import { Renderer2Stub } from './../../testing/stubs/renderer2.stub';
import { ContentLayoutComponent } from './content-layout.component';

describe('ContentLayoutComponent', () => {
    let component: ContentLayoutComponent;
    let fixture: ComponentFixture<ContentLayoutComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [RouterTestingModule],
            declarations: [ContentLayoutComponent],
            providers: [
                LayoutTemplateStore,
                LayoutService,
                BodyClassesUpdateService,
                ChangeDetectorRef,
                { provide: Renderer2, useClass: Renderer2Stub }
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ContentLayoutComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
