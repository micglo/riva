using Riva.AnnouncementPreferences.Core.Enums;

namespace Riva.AnnouncementPreferences.Core.Extensions
{
    public static class NumberOfRoomsExtension
    {
        public static int? ConvertToInt(this NumberOfRooms? numberOfRooms)
        {
            if (!numberOfRooms.HasValue)
                return null;

            return numberOfRooms switch
            {
                NumberOfRooms.One => 1,
                NumberOfRooms.Two => 2,
                NumberOfRooms.Three => 3,
                NumberOfRooms.Four => 4,
                _ => 5
            };
        }
    }
}