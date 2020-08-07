import { AnnouncementPreferenceType } from './../enums/announcement-preference-type.enum';

export interface IntegrationEvent {
    correlationId: string;
    creationDate: Date;
}

export interface AccountCreationCompletedIntegrationEvent extends IntegrationEvent {
    accountId: string;
}

export interface AccountDeletionCompletedIntegrationEvent extends IntegrationEvent {
    accountId: string;
}

export interface UserAnnouncementPreferenceCreationCompletedIntegrationEvent extends IntegrationEvent {
    userId: string;
    announcementPreferenceId: string;
    announcementPreferenceType: AnnouncementPreferenceType;
}

export interface UserAnnouncementPreferenceDeletionCompletedIntegrationEvent extends IntegrationEvent {
    userId: string;
    announcementPreferenceId: string;
    announcementPreferenceType: AnnouncementPreferenceType;
}

export interface UserAnnouncementPreferenceUpdateCompletedIntegrationEvent extends IntegrationEvent {
    userId: string;
    announcementPreferenceId: string;
    announcementPreferenceType: AnnouncementPreferenceType;
}

export interface UserUpdateCompletedIntegrationEvent extends IntegrationEvent {
    userId: string;
}
