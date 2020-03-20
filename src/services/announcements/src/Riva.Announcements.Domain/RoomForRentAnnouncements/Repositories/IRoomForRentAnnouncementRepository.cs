using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories
{
    public interface IRoomForRentAnnouncementRepository
    {
        Task<List<RoomForRentAnnouncement>> GetAllAsync();

        Task<List<RoomForRentAnnouncement>> FindAsync(int? pageNumber, int? pageSize, string sort, Guid? cityId,
            DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom, decimal? priceTo,
            Guid? cityDistrict, RoomTypeEnumeration roomType);
        Task<RoomForRentAnnouncement> GetByIdAsync(Guid id);
        Task AddAsync(RoomForRentAnnouncement roomForRentAnnouncement);
        Task UpdateAsync(RoomForRentAnnouncement roomForRentAnnouncement);
        Task DeleteAsync(RoomForRentAnnouncement roomForRentAnnouncement);
        Task<int> CountAsync();
        Task<int> CountAsync(Guid? cityId, DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom,
            decimal? priceTo, Guid? cityDistrict, RoomTypeEnumeration roomType);
    }
}