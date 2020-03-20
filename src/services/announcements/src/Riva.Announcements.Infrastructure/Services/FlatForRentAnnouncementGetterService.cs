using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Announcements.Infrastructure.Services
{
    public class FlatForRentAnnouncementGetterService : IFlatForRentAnnouncementGetterService
    {
        private readonly IFlatForRentAnnouncementRepository _flatForRentAnnouncementRepository;

        public FlatForRentAnnouncementGetterService(IFlatForRentAnnouncementRepository flatForRentAnnouncementRepository)
        {
            _flatForRentAnnouncementRepository = flatForRentAnnouncementRepository;
        }

        public async Task<GetResult<FlatForRentAnnouncement>> GetByIdAsync(Guid id)
        {
            var flatForRentAnnouncement = await _flatForRentAnnouncementRepository.GetByIdAsync(id);
            if (flatForRentAnnouncement is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(FlatForRentAnnouncementErrorCodeEnumeration.NotFound, FlatForRentAnnouncementErrorMessage.NotFound)
                };
                return GetResult<FlatForRentAnnouncement>.Fail(errors);
            }
            return GetResult<FlatForRentAnnouncement>.Ok(flatForRentAnnouncement);
        }
    }
}