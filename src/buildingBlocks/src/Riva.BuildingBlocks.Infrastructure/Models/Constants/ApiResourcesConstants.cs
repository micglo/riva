namespace Riva.BuildingBlocks.Infrastructure.Models.Constants
{
    public static class ApiResourcesConstants
    {
        public static readonly ApiResource RivaIdentityApiResource = new ApiResource("RivaIdentity", "Riva Identity");
        public static readonly ApiResource RivaAdministrativeDivisionsApiResource = new ApiResource("RivaAdministrativeDivisions", "Riva AdministrativeDivisions");
        public static readonly ApiResource RivaAnnouncementsApiResource = new ApiResource("RivaAnnouncements", "Riva Announcements");
        public static readonly ApiResource RivaUsersApiResource = new ApiResource("RivaUsers", "Riva Users");
        public static readonly ApiResource RivaSignalRApiResource = new ApiResource("RivaSignalR", "Riva SignalR");
    }
}