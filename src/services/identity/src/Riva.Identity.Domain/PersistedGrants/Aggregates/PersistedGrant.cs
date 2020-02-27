using System;
using Riva.Identity.Domain.PersistedGrants.Builders;
using Riva.Identity.Domain.PersistedGrants.ValueObjects;

namespace Riva.Identity.Domain.PersistedGrants.Aggregates
{
    public class PersistedGrant
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public string Data { get; set; }
        public DateTime? Expiration { get; set; }

        private PersistedGrant(PersistedGrantBuilder builder)
        {
            Key = builder.Key;
            Type = builder.Type;
            SubjectId = builder.SubjectId;
            ClientId = builder.ClientId;
            CreationTime = builder.CreationTime;
            Data = builder.Data;
            Expiration = builder.Expiration;
        }

        public static IPersistedGrantKeySetter Builder()
        {
            return new PersistedGrantBuilder();
        }

        private class PersistedGrantBuilder : IPersistedGrantKeySetter, IPersistedGrantTypeSetter, 
            IPersistedGrantSubjectIdSetter, IPersistedGrantClientIdSetter, IPersistedGrantCreationTimeSetter, 
            IPersistedGrantDataSetter, IPersistedGrantBuilder
        {
            public string Key { get; private set; }
            public string Type { get; private set; }
            public Guid SubjectId { get; private set; }
            public Guid ClientId { get; private set; }
            public DateTime CreationTime { get; private set; }
            public string Data { get; private set; }
            public DateTime? Expiration { get; private set; }

            public IPersistedGrantTypeSetter SetKey(string key)
            {
                Key = new PersistedGrantKey(key);
                return this;
            }

            public IPersistedGrantSubjectIdSetter SetType(string type)
            {
                Type = new PersistedGrantType(type);
                return this;
            }

            public IPersistedGrantClientIdSetter SetSubjectId(Guid subjectId)
            {
                SubjectId = new PersistedGrantSubjectId(subjectId);
                return this;
            }

            public IPersistedGrantCreationTimeSetter SetClientId(Guid clientId)
            {
                ClientId = new PersistedGrantClientId(clientId);
                return this;
            }

            public IPersistedGrantDataSetter SetCreationTime(DateTime creationTime)
            {
                CreationTime = new PersistedGrantCreationTime(creationTime);
                return this;
            }

            public IPersistedGrantBuilder SetData(string data)
            {
                Data = new PersistedGrantData(data);
                return this;
            }

            public IPersistedGrantBuilder SetExpiration(DateTime? expiration)
            {
                Expiration = new PersistedGrantExpiration(expiration, CreationTime);
                return this;
            }

            public PersistedGrant Build()
            {
                return new PersistedGrant(this);
            }
        }
    }
}