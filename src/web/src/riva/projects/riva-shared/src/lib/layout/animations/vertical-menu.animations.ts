import { animate, state, style, transition, trigger } from '@angular/animations';

const animationTiming = 200;
export const VERTICAL_MENU_ANIMATIONS = [
    trigger('slideInOut', [
        state('1', style({ height: '*' })),
        state('0', style({ height: '0px' })),
        transition('1 <=> 0', animate(animationTiming))
    ])
];
