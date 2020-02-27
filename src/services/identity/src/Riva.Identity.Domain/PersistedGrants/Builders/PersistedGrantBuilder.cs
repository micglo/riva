using System;
using Riva.Identity.Domain.PersistedGrants.Aggregates;

namespace Riva.Identity.Domain.PersistedGrants.Builders
{
    public interface IPersistedGrantKeySetter
    {
        IPersistedGrantTypeSetter SetKey(string key);
    }

    public interface IPersistedGrantTypeSetter
    {
        IPersistedGrantSubjectIdSetter SetType(string type);
    }

    public interface IPersistedGrantSubjectIdSetter
    {
        IPersistedGrantClientIdSetter SetSubjectId(Guid subjectId);
    }

    public interface IPersistedGrantClientIdSetter
    {
        IPersistedGrantCreationTimeSetter SetClientId(Guid clientId);
    }

    public interface IPersistedGrantCreationTimeSetter
    {
        IPersistedGrantDataSetter SetCreationTime(DateTime creationTime);
    }

    public interface IPersistedGrantDataSetter
    {
        IPersistedGrantBuilder SetData(string data);
    }

    public interface IPersistedGrantBuilder
    {
        IPersistedGrantBuilder SetExpiration(DateTime? expiration);
        PersistedGrant Build();
    }
}