import { ValueProvider } from '@angular/core';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { DEFAULT_PERFECT_SCROLLBAR_CONFIG } from './default-perfect-scrollbar-config';

export const PERFECT_SCROLLBAR_CONFIG_PROVIDER: ValueProvider = {
    provide: PERFECT_SCROLLBAR_CONFIG,
    useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
};
