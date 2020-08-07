export interface FlatForRentAnnouncementPreference {
    id: string;
    cityId: string;
    priceMin: number | null;
    priceMax: number | null;
    roomNumbersMin: number | null;
    roomNumbersMax: number | null;
    cityDistricts: Array<string>;
}
