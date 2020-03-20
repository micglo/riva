using System;

namespace Riva.Announcements.Core.Models
{
    public interface ICity
    {
        public Guid Id { get; }
        public byte[] RowVersion { get; }
        public string Name { get; }
        public string PolishName { get; }
        public Guid StateId { get; }
    }
}