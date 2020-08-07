import { Observable, of } from 'rxjs';
import { AnnouncementSendingFrequency } from './../enums/announcement-sending-frequency.enum';
import { FlatForRentAnnouncementPreference } from './../models/flat-for-rent-announcement-preference.model';
import { RoomForRentAnnouncementPreference } from './../models/room-for-rent-announcement-preference.model';
import { User } from './../models/user.model';

export class UserServiceStub {
    public get user$(): Observable<User> {
        const user = {
            id: 'id',
            email: 'email@email.com',
            picture: 'picture',
            serviceActive: true,
            announcementPreferenceLimit: 2,
            announcementSendingFrequency: AnnouncementSendingFrequency.EveryFourHours,
            roomForRentAnnouncementPreferences: new Array<RoomForRentAnnouncementPreference>(),
            flatForRentAnnouncementPreferences: new Array<FlatForRentAnnouncementPreference>()
        };
        return of(user);
    }

    public loadUser(id: string): void {}
}
