using System;
using System.Collections.Generic;

namespace Riva.Announcements.Core.Models
{
    public interface ICityDistrict
    {
        public Guid Id { get; }
        public byte[] RowVersion { get; }
        public string Name { get; }
        public string PolishName { get; }
        public Guid CityId { get; }
        public Guid? ParentId { get; }
        public IReadOnlyCollection<string> NameVariants { get; }
    }
}