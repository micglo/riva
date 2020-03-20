using System;
using AutoMapper;
using Riva.Announcements.Core.Commands;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;

namespace Riva.Announcements.Infrastructure.CoreAutoMapperProfiles
{
    public class RoomForRentAnnouncementProfile : Profile
    {
        public RoomForRentAnnouncementProfile()
        {
            CreateMap<RoomForRentAnnouncement, RoomForRentAnnouncementOutputQuery>();

            CreateMap<CreateRoomForRentAnnouncementCommand, RoomForRentAnnouncement>()
                .ForMember(x => x.CityDistricts, opt => opt.Ignore())
                .ForMember(x => x.RoomTypes, opt => opt.Ignore())
                .ConstructUsing(x => RoomForRentAnnouncement.Builder()
                    .SetId(x.RoomForRentAnnouncementId)
                    .SetTitle(x.Title)
                    .SetSourceUrl(x.SourceUrl)
                    .SetCityId(x.CityId)
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription(x.Description)
                    .SetPrice(x.Price)
                    .SetCityDistricts(x.CityDistricts)
                    .SetRoomTypes(x.RoomTypes)
                    .Build());
        }
    }
}