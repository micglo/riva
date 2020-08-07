import { ValueProvider } from '@angular/core';
import { DEFAULT_TRANSLATION_OPTIONS } from './default-translation-options';
import { TRANSLATION_OPTIONS } from './translation-options-injection-token';

export const TRANSLATION_OPTIONS_PROVIDER: ValueProvider = {
    provide: TRANSLATION_OPTIONS,
    useValue: DEFAULT_TRANSLATION_OPTIONS
};
