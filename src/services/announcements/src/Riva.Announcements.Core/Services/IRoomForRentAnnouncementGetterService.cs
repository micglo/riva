using System;
using System.Threading.Tasks;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Announcements.Core.Services
{
    public interface IRoomForRentAnnouncementGetterService
    {
        Task<GetResult<RoomForRentAnnouncement>> GetByIdAsync(Guid id);
    }
}