using System;
using Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.FlatForRentAnnouncementPreferenceValueObjects
{
    public class FlatForRentAnnouncementPreferenceId
    {
        private readonly Guid _id;

        public FlatForRentAnnouncementPreferenceId(Guid id)
        {
            if (id == Guid.Empty || id == new Guid())
                throw new FlatForRentAnnouncementPreferenceIdNullException();

            _id = id;
        }

        public static implicit operator Guid(FlatForRentAnnouncementPreferenceId id)
        {
            return id._id;
        }
    }
}