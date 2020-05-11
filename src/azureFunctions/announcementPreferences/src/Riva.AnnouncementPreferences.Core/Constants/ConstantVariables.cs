namespace Riva.AnnouncementPreferences.Core.Constants
{
    public class ConstantVariables
    {
        public const string CreateFlatForRentAnnouncementPreferenceFunctionName = "CreateFlatForRentAnnouncementPreference";
        public const string UpdateFlatForRentAnnouncementPreferenceFunctionName = "UpdateFlatForRentAnnouncementPreference";
        public const string CreateRoomForRentAnnouncementPreferenceFunctionName = "CreateRoomForRentAnnouncementPreference";
        public const string UpdateRoomForRentAnnouncementPreferenceFunctionName = "UpdateRoomForRentAnnouncementPreference";
        public const string DeleteAnnouncementPreferenceFunctionName = "DeleteAnnouncementPreference";
        public const string UpdateAnnouncementPreferencesFunctionName = "UpdateAnnouncementPreferences";
        public const string DeleteAnnouncementPreferencesFunctionName = "DeleteAnnouncementPreferences";
        public const string MatchFlatForRentAnnouncementsFunctionName = "MatchFlatForRentAnnouncements";
        public const string MatchRoomForRentAnnouncementsFunctionName = "MatchRoomForRentAnnouncements";
        public const string SendAnnouncementUrlsEveryHourFunctionName = "SendAnnouncementUrlsEveryHour";
        public const string SendAnnouncementUrlsEveryTwoHoursFunctionName = "SendAnnouncementUrlsEveryTwoHours";
        public const string SendAnnouncementUrlsEveryThreeHoursFunctionName = "SendAnnouncementUrlsEveryThreeHours";
        public const string SendAnnouncementUrlsEveryFourHoursFunctionName = "SendAnnouncementUrlsEveryFourHours";
        public const string SendAnnouncementUrlsEveryFiveHoursFunctionName = "SendAnnouncementUrlsEveryFiveHours";
        public const string SendAnnouncementUrlsEverySixHoursFunctionName = "SendAnnouncementUrlsEverySixHours";
        public const string SendAnnouncementUrlsEveryTwelveHoursFunctionName = "SendAnnouncementUrlsEveryTwelveHours";

        public const string ServiceBusConnectionStringName = "ServiceBusConnectionString";
        public const string ServiceBusTopicName = "riva-topic";
        public const string CreateFlatForRentAnnouncementPreferenceSubscriptionName = "create-flat-announcement-preference-subscription";
        public const string UpdateFlatForRentAnnouncementPreferenceSubscriptionName = "update-flat-announcement-preference-subscription";
        public const string CreateRoomForRentAnnouncementPreferenceSubscriptionName = "create-room-announcement-preference-subscription";
        public const string UpdateRoomForRentAnnouncementPreferenceSubscriptionName = "update-room-announcement-preference-subscription";
        public const string DeleteAnnouncementPreferenceSubscriptionName = "delete-announcement-preference-subscription";
        public const string UpdateAnnouncementPreferencesSubscriptionName = "update-announcement-preferences-subscription";
        public const string DeleteAnnouncementPreferencesSubscriptionName = "delete-announcement-preferences-subscription";
        public const string MatchFlatForRentAnnouncementsSubscriptionName = "riva-match-flat-announcements-subscription";
        public const string MatchRoomForRentAnnouncementsSubscriptionName = "riva-match-room-announcements-subscription";

        public const string CosmosCollectionName = "announcementPreferences";
        public const string IntegrationEventSuffix = "IntegrationEvent";

        public const string CosmosDbDatabaseNameVariableName = "CosmosDbDatabaseName";
        public const string CosmosDbUriVariableName = "CosmosDbUri";
        public const string CosmosDbAuthKeyVariableName = "CosmosDbAuthKey";
        public const string CosmosDbCollectionThroughputVariableName = "CosmosDbCollectionThroughput";
        public const string CosmosDbCollectionPartitionKey = "/_partitionKey";
        public const string SendGridApiKeyVariableName = "SendGridApiKey";

        public const string SendAnnouncementUrlsEveryHourCronExpr = "0 0 * * * *";
        public const string SendAnnouncementUrlsEveryTwoHoursCronExpr = "0 0 0/2 * * *";
        public const string SendAnnouncementUrlsEveryThreeHoursCronExpr = "0 0 0/3 * * *";
        public const string SendAnnouncementUrlsEveryFourHoursCronExpr = "0 0 0/4 * * *";
        public const string SendAnnouncementUrlsEveryFiveHoursCronExpr = "0 0 0/5 * * *";
        public const string SendAnnouncementUrlsEverySixHoursCronExpr = "0 0 0/6 * * *";
        public const string SendAnnouncementUrlsEveryTwelveHoursCronExpr = "0 0 0/12 * * *";
        
        public const string RivaEmailAddress = "riva@riva.com";
        public const string EmailSubject = "Selected announcements";
    }
}