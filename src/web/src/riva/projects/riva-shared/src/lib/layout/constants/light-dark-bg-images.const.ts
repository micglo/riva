import { LightDarkImage } from './../models/light-dark-image.model';
import { SIDEBAR_BG_1_URL, SIDEBAR_BG_2_URL, SIDEBAR_BG_3_URL, SIDEBAR_BG_4_URL, SIDEBAR_BG_5_URL, SIDEBAR_BG_6_URL } from './images.const';

export const LIGHT_DARK_BG_IMAGES = new Array<LightDarkImage>(
    new LightDarkImage(SIDEBAR_BG_1_URL, false),
    new LightDarkImage(SIDEBAR_BG_2_URL, false),
    new LightDarkImage(SIDEBAR_BG_3_URL, false),
    new LightDarkImage(SIDEBAR_BG_4_URL, false),
    new LightDarkImage(SIDEBAR_BG_5_URL, false),
    new LightDarkImage(SIDEBAR_BG_6_URL, false)
);
