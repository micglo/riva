using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.Core.Enumerations
{
    public class ServiceComponentEnumeration : EnumerationBase
    {
        public static ServiceComponentEnumeration RivaIdentity => new ServiceComponentEnumeration(1, nameof(RivaIdentity));
        public static ServiceComponentEnumeration RivaMessages => new ServiceComponentEnumeration(2, nameof(RivaMessages));
        public static ServiceComponentEnumeration RivaAdministrativeDivisions => new ServiceComponentEnumeration(3, nameof(RivaAdministrativeDivisions));
        public static ServiceComponentEnumeration RivaAnnouncements => new ServiceComponentEnumeration(4, nameof(RivaAnnouncements));
        public static ServiceComponentEnumeration RivaUsers => new ServiceComponentEnumeration(5, nameof(RivaUsers));
        public static ServiceComponentEnumeration RivaAnnouncementPreferences => new ServiceComponentEnumeration(6, nameof(RivaAnnouncementPreferences));
        public static ServiceComponentEnumeration RivaSignalR => new ServiceComponentEnumeration(7, nameof(RivaSignalR));

        public ServiceComponentEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}