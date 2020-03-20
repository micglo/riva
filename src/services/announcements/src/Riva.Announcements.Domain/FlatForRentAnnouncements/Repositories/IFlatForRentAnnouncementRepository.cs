using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories
{
    public interface IFlatForRentAnnouncementRepository
    {
        Task<List<FlatForRentAnnouncement>> GetAllAsync();

        Task<List<FlatForRentAnnouncement>> FindAsync(int? pageNumber, int? pageSize, string sort, Guid? cityId,
            DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom, decimal? priceTo,
            Guid? cityDistrict, NumberOfRoomsEnumeration numberOfRooms);
        Task<FlatForRentAnnouncement> GetByIdAsync(Guid id);
        Task AddAsync(FlatForRentAnnouncement flatForRentAnnouncement);
        Task UpdateAsync(FlatForRentAnnouncement flatForRentAnnouncement);
        Task DeleteAsync(FlatForRentAnnouncement flatForRentAnnouncement);
        Task<int> CountAsync();
        Task<int> CountAsync(Guid? cityId, DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom,
            decimal? priceTo, Guid? cityDistrict, NumberOfRoomsEnumeration numberOfRooms);
    }
}