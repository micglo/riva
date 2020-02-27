using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Riva.BuildingBlocks.Domain
{
    public abstract class VersionedAggregateBase : AggregateBase
    {
        private List<byte> _rowVersion;

        public IReadOnlyCollection<byte> RowVersion => _rowVersion.AsReadOnly();

        protected VersionedAggregateBase(Guid id, IEnumerable<byte> rowVersion) : base(id)
        {
            _rowVersion = new VersionedAggregateRowVersion(rowVersion);
        }

        public void SetRowVersion(IEnumerable<byte> rowVersion)
        {
            _rowVersion = new VersionedAggregateRowVersion(rowVersion);
        }
    }

    public class VersionedAggregateRowVersion
    {
        private readonly List<byte> _rowVersion;

        public VersionedAggregateRowVersion(IEnumerable<byte> rowVersion)
        {
            if (rowVersion is null)
                throw new VersionedAggregateRowVersionNullException();

            _rowVersion = new List<byte>(rowVersion.ToList());
        }

        public static implicit operator List<byte>(VersionedAggregateRowVersion rowVersion)
        {
            return rowVersion._rowVersion;
        }
    }

    [Serializable]
    public class VersionedAggregateRowVersionNullException : DomainException
    {
        private const string ErrorMessage = "RowVersion argument is required.";

        public override string Message { get; }

        public VersionedAggregateRowVersionNullException() : base(ErrorMessage)
        {
        }

        public VersionedAggregateRowVersionNullException(string message) : base(message)
        {
        }

        public VersionedAggregateRowVersionNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private VersionedAggregateRowVersionNullException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            Message = ErrorMessage;
        }
    }
}