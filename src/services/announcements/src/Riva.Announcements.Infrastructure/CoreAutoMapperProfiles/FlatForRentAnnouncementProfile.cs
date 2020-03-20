using System;
using AutoMapper;
using Riva.Announcements.Core.Commands;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;

namespace Riva.Announcements.Infrastructure.CoreAutoMapperProfiles
{
    public class FlatForRentAnnouncementProfile : Profile
    {
        public FlatForRentAnnouncementProfile()
        {
            CreateMap<FlatForRentAnnouncement, FlatForRentAnnouncementOutputQuery>();

            CreateMap<CreateFlatForRentAnnouncementCommand, FlatForRentAnnouncement>()
                .ForMember(x => x.CityDistricts, opt => opt.Ignore())
                .ConstructUsing(x => FlatForRentAnnouncement.Builder()
                    .SetId(x.FlatForRentAnnouncementId)
                    .SetTitle(x.Title)
                    .SetSourceUrl(x.SourceUrl)
                    .SetCityId(x.CityId)
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription(x.Description)
                    .SetNumberOfRooms(x.NumberOfRooms)
                    .SetPrice(x.Price)
                    .SetCityDistricts(x.CityDistricts)
                    .Build());
        }
    }
}