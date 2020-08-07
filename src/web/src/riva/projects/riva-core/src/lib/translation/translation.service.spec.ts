import { getLocaleId } from '@angular/common';
import { inject, TestBed } from '@angular/core/testing';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { StorageModule } from './../storage/storage.module';
import { StorageService } from './../storage/storage.service';
import { WindowModule } from './../window/window.module';
import { TranslationLanguage } from './translation-language.enum';
import { TranslationModule } from './translation.module';
import { KEY_LANGUAGE, TranslationService } from './translation.service';

describe('TranslationService', () => {
    let service: TranslationService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [TranslateModule.forRoot(), TranslationModule.forRoot(), StorageModule.forRoot(), WindowModule.forRoot()]
        });
        service = TestBed.inject(TranslationService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('get language should return', () => {
        let translationService: TranslationService;

        beforeEach(() => {
            translationService = TestBed.inject(TranslationService);
        });

        it('value from storage', inject([StorageService], (storageService: StorageService) => {
            const lang = TranslationLanguage.Polish;
            spyOn(storageService, 'getItem').and.returnValue(lang);

            const result = translationService.language;

            expect(result).toBe(lang);
        }));

        it('default value when storage is empty', () => {
            const result = translationService.language;

            expect(result).toBe(TranslationLanguage.English);
        });
    });

    it('get supportedLanguages should return supported languages', () => {
        const expectedLanguages = Object.entries(TranslationLanguage).map((value: [string, TranslationLanguage]) => value[1]);

        const result = service.supportedLanguages;

        expect(result).toEqual(expectedLanguages);
    });

    it('initialize should init service', inject([TranslateService], (translateService: TranslateService) => {
        const lang = TranslationLanguage.English;
        spyOn(translateService, 'addLangs');
        spyOn(translateService, 'setDefaultLang');
        spyOn(translateService, 'use');

        service.initialize();

        expect(translateService.addLangs).toHaveBeenCalledWith(Object.keys(TranslationLanguage));
        expect(translateService.setDefaultLang).toHaveBeenCalledWith(lang);
        expect(translateService.use).toHaveBeenCalledWith(lang);

        const registeredEnLocale = getLocaleId(TranslationLanguage.English);
        const registeredPlLocale = getLocaleId(TranslationLanguage.Polish);

        expect(registeredEnLocale).not.toBeNull();
        expect(registeredPlLocale).not.toBeNull();
    }));

    it('set language should set desired language', inject(
        [TranslationService, TranslateService, StorageService],
        (translationService: TranslationService, translateService: TranslateService, storageService: StorageService) => {
            const lang = TranslationLanguage.Polish;
            spyOn(translateService, 'use');
            spyOn(storageService, 'setItem');

            translationService.setLanguage(lang);

            expect(translateService.use).toHaveBeenCalledWith(lang);
            expect(storageService.setItem).toHaveBeenCalledWith(KEY_LANGUAGE, lang);
        }
    ));

    it('translate should translate given key', inject([TranslateService], (translateService: TranslateService) => {
        const key = 'key';
        const interpolateParams = 'interpolateParams';
        spyOn(translateService, 'instant');

        service.translate(key, interpolateParams);

        expect(translateService.instant).toHaveBeenCalledWith(key, interpolateParams);
    }));
});
