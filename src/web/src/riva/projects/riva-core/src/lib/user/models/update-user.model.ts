import { AnnouncementSendingFrequency } from './../enums/announcement-sending-frequency.enum';

export interface UpdateUser {
    id: string;
    serviceActive: boolean;
    announcementPreferenceLimit: number;
    announcementSendingFrequency: AnnouncementSendingFrequency;
    picture: File;
}
