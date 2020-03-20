using AutoMapper;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Enums;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions;

namespace Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.AutoMapperProfiles
{
    public class FlatForRentAnnouncementProfile : Profile
    {
        public FlatForRentAnnouncementProfile()
        {
            CreateMap<FlatForRentAnnouncementEntity, FlatForRentAnnouncement>()
                .ForMember(x => x.CityDistricts, opt => opt.Ignore())
                .ConvertUsing(x => FlatForRentAnnouncement.Builder()
                    .SetId(x.Id)
                    .SetTitle(x.Title)
                    .SetSourceUrl(x.SourceUrl)
                    .SetCityId(x.CityId)
                    .SetCreated(x.Created)
                    .SetDescription(x.Description)
                    .SetNumberOfRooms(x.NumberOfRooms.ConvertToEnumeration())
                    .SetPrice(x.Price)
                    .SetCityDistricts(x.CityDistricts)
                    .Build());

            CreateMap<FlatForRentAnnouncement, FlatForRentAnnouncementEntity>()
                .ForMember(x => x.NumberOfRooms,
                    opt => opt.MapFrom<NumberOfRoomsEnumerationToNumberOfRoomsEnumValueResolver>());
        }

        private class NumberOfRoomsEnumerationToNumberOfRoomsEnumValueResolver : IValueResolver<FlatForRentAnnouncement, FlatForRentAnnouncementEntity, NumberOfRooms>
        {
            public NumberOfRooms Resolve(FlatForRentAnnouncement source, FlatForRentAnnouncementEntity destination, NumberOfRooms destMember, ResolutionContext context)
            {
                return source.NumberOfRooms.ConvertToEnum();
            }
        }
    }
}