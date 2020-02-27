using System;
using Riva.Identity.Domain.PersistedGrants.Exceptions;

namespace Riva.Identity.Domain.PersistedGrants.ValueObjects
{
    public class PersistedGrantSubjectId
    {
        private readonly Guid _subjectId;

        public PersistedGrantSubjectId(Guid subjectId)
        {
            if (subjectId == Guid.Empty || Equals(subjectId, new Guid?()) || subjectId == new Guid())
                throw new PersistedGrantSubjectIdNullException();

            _subjectId = subjectId;
        }

        public static implicit operator Guid(PersistedGrantSubjectId subjectId)
        {
            return subjectId._subjectId;
        }
    }
}