using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Enums;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions;

namespace Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.AutoMapperProfiles
{
    public class RoomForRentAnnouncementProfile : Profile
    {
        public RoomForRentAnnouncementProfile()
        {
            CreateMap<RoomForRentAnnouncementEntity, RoomForRentAnnouncement>()
                .ForMember(x => x.CityDistricts, opt => opt.Ignore())
                .ForMember(x => x.RoomTypes, opt => opt.Ignore())
                .ConvertUsing(x => RoomForRentAnnouncement.Builder()
                    .SetId(x.Id)
                    .SetTitle(x.Title)
                    .SetSourceUrl(x.SourceUrl)
                    .SetCityId(x.CityId)
                    .SetCreated(x.Created)
                    .SetDescription(x.Description)
                    .SetPrice(x.Price)
                    .SetCityDistricts(x.CityDistricts)
                    .SetRoomTypes(x.RoomTypes.Select(r => r.ConvertToEnumeration()))
                    .Build());

            CreateMap<RoomForRentAnnouncement, RoomForRentAnnouncementEntity>()

                .ForMember(x => x.RoomTypes,
                    opt => opt.MapFrom<RoomTypeEnumerationToRoomTypeEnumValueResolver>());
        }

        private class RoomTypeEnumerationToRoomTypeEnumValueResolver : IValueResolver<RoomForRentAnnouncement, RoomForRentAnnouncementEntity, IEnumerable<RoomType>>
        {
            public IEnumerable<RoomType> Resolve(RoomForRentAnnouncement source, RoomForRentAnnouncementEntity destination, IEnumerable<RoomType> destMember, ResolutionContext context)
            {
                return source.RoomTypes.Select(x => x.ConvertToEnum());
            }
        }
    }
}