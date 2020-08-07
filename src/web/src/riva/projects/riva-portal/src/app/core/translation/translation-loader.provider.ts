import { ClassProvider } from '@angular/core';
import { TranslateLoader } from '@ngx-translate/core';
import { TranslationLoader } from './translation-loader';

export const TRANSLATION_LOADER_PROVIDER: ClassProvider = {
    provide: TranslateLoader,
    useClass: TranslationLoader
};
