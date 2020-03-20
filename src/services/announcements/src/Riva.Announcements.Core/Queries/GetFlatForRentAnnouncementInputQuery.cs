using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries
{
    public class GetFlatForRentAnnouncementInputQuery : IInputQuery
    {
        public Guid FlatForRentAnnouncementId { get; }

        public GetFlatForRentAnnouncementInputQuery(Guid flatForRentAnnouncementId)
        {
            FlatForRentAnnouncementId = flatForRentAnnouncementId;
        }
    }
}