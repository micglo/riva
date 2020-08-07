import { async, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule } from '@ngx-translate/core';
import { AuthServiceStub, StorageModule, WindowModule } from 'riva-core';
import { AuthService, TranslationModule } from 'riva-core';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                RouterTestingModule,
                TranslateModule.forRoot(),
                TranslationModule.forRoot(),
                StorageModule.forRoot(),
                WindowModule.forRoot()
            ],
            providers: [{ provide: AuthService, useClass: AuthServiceStub }],
            declarations: [AppComponent]
        }).compileComponents();
    }));

    it('should create the app', () => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.componentInstance;
        expect(app).toBeTruthy();
    });
});
