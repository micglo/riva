import { AnnouncementSendingFrequency } from './../enums/announcement-sending-frequency.enum';
import { FlatForRentAnnouncementPreference } from './flat-for-rent-announcement-preference.model';
import { RoomForRentAnnouncementPreference } from './room-for-rent-announcement-preference.model';

export interface User {
    id: string;
    email: string;
    picture: string;
    serviceActive: boolean;
    announcementPreferenceLimit: number;
    announcementSendingFrequency: AnnouncementSendingFrequency;
    roomForRentAnnouncementPreferences: Array<RoomForRentAnnouncementPreference>;
    flatForRentAnnouncementPreferences: Array<FlatForRentAnnouncementPreference>;
}
