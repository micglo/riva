using System;
using Riva.Identity.Domain.PersistedGrants.Exceptions;

namespace Riva.Identity.Domain.PersistedGrants.ValueObjects
{
    public class PersistedGrantCreationTime
    {
        private readonly DateTime _creationTime;

        public PersistedGrantCreationTime(DateTime creationTime)
        {
            if (creationTime == DateTime.MinValue)
                throw new PersistedGrantCreationTimeMinValueException();

            _creationTime = creationTime;
        }

        public static implicit operator DateTime(PersistedGrantCreationTime creationTime)
        {
            return creationTime._creationTime;
        }
    }
}