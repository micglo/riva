import { RoomType } from './../enums/room-type.enum';

export interface RoomForRentAnnouncementPreference {
    id: string;
    cityId: string;
    priceMin: number | null;
    priceMax: number | null;
    roomType: RoomType | null;
    cityDistricts: Array<string>;
}
