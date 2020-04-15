using System;
using Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.RoomForRentAnnouncementPreferenceValueObjects
{
    public class RoomForRentAnnouncementPreferenceId
    {
        private readonly Guid _id;

        public RoomForRentAnnouncementPreferenceId(Guid id)
        {
            if (id == Guid.Empty || id == new Guid())
                throw new RoomForRentAnnouncementPreferenceIdNullException();

            _id = id;
        }

        public static implicit operator Guid(RoomForRentAnnouncementPreferenceId id)
        {
            return id._id;
        }
    }
}