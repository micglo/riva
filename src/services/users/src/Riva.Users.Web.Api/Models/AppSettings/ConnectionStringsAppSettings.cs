namespace Riva.Users.Web.Api.Models.AppSettings
{
    public class ConnectionStringsAppSettings : BuildingBlocks.WebApi.Models.AppSettings.ConnectionStringsAppSettings
    {
        public string RivaUsersSQLServerDatabaseConnectionString { get; set; }
        public string StorageAccountConnectionString { get; set; }
    }
}