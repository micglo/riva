namespace Riva.AnnouncementPreferences.Core.Models
{
    public class AppSettings
    {
        public string CosmosDbDatabaseName { get; set; }
        public string CosmosDbCollectionThroughput { get; set; }
        public string MaxDegreeOfParallelism { get; set; }
    }
}