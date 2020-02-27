using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Riva.BuildingBlocks.Domain
{
    public abstract class EnumerationBase : IComparable
    {
        public int Value { get; }
        public string DisplayName { get; }

        protected EnumerationBase(int value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public override string ToString() => DisplayName;

        public static IEnumerable<T> GetAll<T>() where T : EnumerationBase
        {
            var fields = typeof(T).GetProperties(BindingFlags.Public |
                                                 BindingFlags.Static |
                                                 BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            return obj is EnumerationBase enumeration && Equals(enumeration);
        }

        protected bool Equals(EnumerationBase other)
        {
            return string.Equals(DisplayName, other.DisplayName) && Value == other.Value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DisplayName != null ? DisplayName.GetHashCode() : 0) * 397) ^ Value;
            }
        }

        public int CompareTo(object other) => Value.CompareTo(((EnumerationBase)other).Value);
    }
}