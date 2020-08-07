import { registerLocaleData } from '@angular/common';
import localeEn from '@angular/common/locales/en-US-POSIX';
import localePl from '@angular/common/locales/pl';
import { EventEmitter, Inject, Injectable } from '@angular/core';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { StorageService } from './../storage/storage.service';
import { TranslationLanguage } from './translation-language.enum';
import { TranslationOptions } from './translation-options';
import { TRANSLATION_OPTIONS } from './translation-options-injection-token';

export const KEY_LANGUAGE = 'language';

@Injectable()
export class TranslationService {
    private _isInitialized = false;

    public get language(): TranslationLanguage {
        return this.storageService.getItem(KEY_LANGUAGE) || this.translationOptions.defaultLanguage;
    }

    public get supportedLanguages(): TranslationLanguage[] {
        return Object.entries(TranslationLanguage).map((value: [string, TranslationLanguage]) => value[1]);
    }

    public get onLanguageChange(): EventEmitter<LangChangeEvent> {
        return this.translateService.onLangChange;
    }

    constructor(
        @Inject(TRANSLATION_OPTIONS) private translationOptions: TranslationOptions,
        private translateService: TranslateService,
        private storageService: StorageService
    ) {}

    public initialize(): void {
        if (!this._isInitialized) {
            const language = this.language;
            this.translateService.addLangs(Object.keys(TranslationLanguage));
            this.translateService.setDefaultLang(language);
            this.translateService.use(language);
            this.registerLocale();
            this._isInitialized = true;
        }
    }

    public setLanguage(lang: TranslationLanguage): void {
        this.translateService.use(lang);
        this.storageService.setItem(KEY_LANGUAGE, lang);
    }

    // tslint:disable-next-line: ban-types
    public translate(key: string | Array<string>, interpolateParams?: Object): string {
        return this.translateService.instant(key, interpolateParams);
    }

    private registerLocale(): void {
        registerLocaleData(localePl, TranslationLanguage.Polish);
        registerLocaleData(localeEn, TranslationLanguage.English);
    }
}
