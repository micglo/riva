using System;
using System.Threading.Tasks;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Announcements.Core.Services
{
    public interface IFlatForRentAnnouncementGetterService
    {
        Task<GetResult<FlatForRentAnnouncement>> GetByIdAsync(Guid id);
    }
}