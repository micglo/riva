namespace Riva.Identity.Web.Models.AppSettings
{
    public class ConnectionStringsAppSettings : BuildingBlocks.WebApi.Models.AppSettings.ConnectionStringsAppSettings
    {
        public string RivaIdentitySQLServerDatabaseConnectionString { get; set; }
    }
}