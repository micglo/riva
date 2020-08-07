import { AnnouncementPreferenceType } from './../enums/announcement-preference-type.enum';
import { IntegrationEvent } from './integration-event.model';

export interface IntegrationEventFailure extends IntegrationEvent {
    reason: string;
    code: string;
}

export interface AccountCreationCompletedIntegrationEventFailure extends IntegrationEventFailure {
    accountId: string;
}

export interface AccountDeletionCompletedIntegrationEventFailure extends IntegrationEventFailure {
    accountId: string;
}

export interface UserAnnouncementPreferenceCreationCompletedIntegrationEventFailure extends IntegrationEventFailure {
    userId: string;
    announcementPreferenceId: string;
    announcementPreferenceType: AnnouncementPreferenceType;
}

export interface UserAnnouncementPreferenceDeletionCompletedIntegrationEventFailure extends IntegrationEventFailure {
    userId: string;
    announcementPreferenceId: string;
    announcementPreferenceType: AnnouncementPreferenceType;
}

export interface UserAnnouncementPreferenceUpdateCompletedIntegrationEventFailure extends IntegrationEventFailure {
    userId: string;
    announcementPreferenceId: string;
    announcementPreferenceType: AnnouncementPreferenceType;
}

export interface UserUpdateCompletedIntegrationEventFailure extends IntegrationEventFailure {
    userId: string;
}
