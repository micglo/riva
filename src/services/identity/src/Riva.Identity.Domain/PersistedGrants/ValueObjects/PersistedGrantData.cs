using Riva.Identity.Domain.PersistedGrants.Exceptions;

namespace Riva.Identity.Domain.PersistedGrants.ValueObjects
{
    public class PersistedGrantData
    {
        private readonly string _data;

        public PersistedGrantData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new PersistedGrantDataNullException();

            _data = data;
        }

        public static implicit operator string(PersistedGrantData data)
        {
            return data._data;
        }
    }
}