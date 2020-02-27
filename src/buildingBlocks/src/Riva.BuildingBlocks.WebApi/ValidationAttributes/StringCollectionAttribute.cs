using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Riva.BuildingBlocks.WebApi.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StringCollectionAttribute : ValidationAttribute
    {
        private readonly bool _allowEmptyCollection;
        private readonly bool _allowNullOrEmptyValues;
        private readonly bool _allowDuplicatedValues;

        public StringCollectionAttribute(bool allowEmptyCollection, bool allowNullOrEmptyValues, bool allowDuplicatedValues)
        {
            _allowEmptyCollection = allowEmptyCollection;
            _allowNullOrEmptyValues = allowNullOrEmptyValues;
            _allowDuplicatedValues = allowDuplicatedValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var collection = (ICollection<string>)value;

                if (!_allowEmptyCollection && !collection.Any())
                    return new ValidationResult($"The field {validationContext.DisplayName} cannot be empty.");

                if (collection.Any())
                {
                    if (!_allowNullOrEmptyValues)
                    {
                        if (collection.Any(string.IsNullOrWhiteSpace))
                            return new ValidationResult($"The field {validationContext.DisplayName} cannot have empty values.");
                    }

                    if (!_allowDuplicatedValues)
                    {
                        var anyDuplicate = collection.GroupBy(x => x).Any(g => g.Count() > 1);
                        if (anyDuplicate)
                            return new ValidationResult($"The field {validationContext.DisplayName} cannot have duplicated values.");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}