import { TranslationLanguage } from 'riva-core';
import { Language } from './../enums/language.enum';

export interface LanguageItem {
    languageName: Language;
    translationLanguage: TranslationLanguage;
    flagUrl: string;
}
