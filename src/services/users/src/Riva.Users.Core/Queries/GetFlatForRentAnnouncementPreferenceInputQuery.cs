using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Users.Core.Queries
{
    public class GetFlatForRentAnnouncementPreferenceInputQuery : IInputQuery
    {
        public Guid UserId { get; }
        public Guid FlatForRentAnnouncementPreferenceId { get; }

        public GetFlatForRentAnnouncementPreferenceInputQuery(Guid userId, Guid flatForRentAnnouncementPreferenceId)
        {
            UserId = userId;
            FlatForRentAnnouncementPreferenceId = flatForRentAnnouncementPreferenceId;
        }
    }
}