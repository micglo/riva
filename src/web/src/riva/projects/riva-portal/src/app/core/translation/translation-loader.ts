import { TranslateLoader } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';

export class TranslationLoader implements TranslateLoader {
    // tslint:disable-next-line: no-any
    getTranslation(lang: string): Observable<any> {
        return from(import(`../../../assets/i18n/${lang}.json`));
    }
}
