namespace Riva.Announcements.Functions
{
    public class ConstantVariables
    {
        public const string CreateAnnouncementFunctionName = "CreateAnnouncement";

        public const string ServiceBusConnectionStringName = "ServiceBusConnectionString";
        public const string ServiceBusTopicName = "riva-topic";
        public const string CreateAnnouncementSubscriptionName = "riva-create-announcement-subscription";

        public const string CosmosDbDatabaseName = "RivaAnnouncements";
        public const string CosmosDbCollectionName = "announcements";
        public const string CosmosDbConnectionStringName = "CosmosDbConnectionString";
    }
}