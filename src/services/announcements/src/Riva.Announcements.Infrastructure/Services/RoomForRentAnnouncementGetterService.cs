using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Announcements.Infrastructure.Services
{
    public class RoomForRentAnnouncementGetterService : IRoomForRentAnnouncementGetterService
    {
        private readonly IRoomForRentAnnouncementRepository _roomForRentAnnouncementRepository;

        public RoomForRentAnnouncementGetterService(IRoomForRentAnnouncementRepository roomForRentAnnouncementRepository)
        {
            _roomForRentAnnouncementRepository = roomForRentAnnouncementRepository;
        }

        public async Task<GetResult<RoomForRentAnnouncement>> GetByIdAsync(Guid id)
        {
            var roomForRentAnnouncement = await _roomForRentAnnouncementRepository.GetByIdAsync(id);
            if (roomForRentAnnouncement is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(RoomForRentAnnouncementErrorCodeEnumeration.NotFound, RoomForRentAnnouncementErrorMessage.NotFound)
                };
                return GetResult<RoomForRentAnnouncement>.Fail(errors);
            }
            return GetResult<RoomForRentAnnouncement>.Ok(roomForRentAnnouncement);
        }
    }
}