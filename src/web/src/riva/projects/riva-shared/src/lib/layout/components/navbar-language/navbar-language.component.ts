import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { TranslationLanguage, TranslationService } from 'riva-core';
import { FLAG_URL } from './../../constants/images.const';
import { Language } from './../../enums/language.enum';
import { LanguageItem } from './../../models/language-item.model';

@Component({
    selector: 'lib-navbar-language',
    templateUrl: './navbar-language.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavbarLanguageComponent implements OnInit {
    private _languages: Array<LanguageItem>;
    private _selectedLanguage: LanguageItem;

    public get languages(): Array<LanguageItem> {
        return this._languages;
    }

    public get selectedLanguage(): LanguageItem {
        return this._selectedLanguage;
    }

    constructor(private translationService: TranslationService) {}

    public ngOnInit(): void {
        this._languages = this.translationService.supportedLanguages.map((l: TranslationLanguage) => this.mapLanguage(l));
        this._selectedLanguage = this.mapLanguage(this.translationService.language);
    }

    public changeLanguage(language: TranslationLanguage): void {
        this.translationService.setLanguage(language);
        this._selectedLanguage = this.mapLanguage(language);
    }

    private mapLanguage(language: TranslationLanguage): LanguageItem {
        return {
            languageName: language === TranslationLanguage.English ? Language.English : Language.Polish,
            translationLanguage: language,
            flagUrl: `${FLAG_URL}${language}.png`
        };
    }
}
