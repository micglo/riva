import { async, ComponentFixture, inject, TestBed } from '@angular/core/testing';
import { TranslateModule } from '@ngx-translate/core';
import { StorageModule, TranslationLanguage, TranslationModule, TranslationService, WindowModule } from 'riva-core';
import { FLAG_URL } from './../../constants/images.const';
import { Language } from './../../enums/language.enum';
import { LanguageItem } from './../../models/language-item.model';
import { NavbarLanguageComponent } from './navbar-language.component';

describe('NavbarLanguageComponent', () => {
    let component: NavbarLanguageComponent;
    let fixture: ComponentFixture<NavbarLanguageComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [TranslateModule.forRoot(), TranslationModule.forRoot(), StorageModule.forRoot(), WindowModule.forRoot()],
            declarations: [NavbarLanguageComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(NavbarLanguageComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('languages should return collection of supported languages for translations', () => {
        const supportedLanguages = Object.entries(TranslationLanguage).map((value: [string, TranslationLanguage]) => value[1]);
        const expectedLanguages = supportedLanguages.map((l: TranslationLanguage) => mapLanguage(l));

        expect(component.languages).toEqual(expectedLanguages);
    });

    it('selectedLanguage should return currently selected language', () => {
        const language = TranslationLanguage.English;
        const expectedSelectedLanguage = mapLanguage(language);

        expect(component.selectedLanguage).toEqual(expectedSelectedLanguage);
    });

    it('changeLanguage should change currently used language', inject([TranslationService], (translationService: TranslationService) => {
        const language = TranslationLanguage.Polish;
        spyOn(translationService, 'setLanguage');
        const expectedSelectedLanguage = mapLanguage(language);

        component.changeLanguage(language);

        expect(translationService.setLanguage).toHaveBeenCalledWith(language);
        expect(component.selectedLanguage).toEqual(expectedSelectedLanguage);
    }));

    function mapLanguage(language: TranslationLanguage): LanguageItem {
        return {
            languageName: language === TranslationLanguage.English ? Language.English : Language.Polish,
            translationLanguage: language,
            flagUrl: `${FLAG_URL}${language}.png`
        };
    }
});
