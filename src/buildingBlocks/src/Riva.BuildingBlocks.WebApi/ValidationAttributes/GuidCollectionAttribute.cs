using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Riva.BuildingBlocks.WebApi.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GuidCollectionAttribute : ValidationAttribute
    {
        private readonly bool _allowEmptyCollection;
        private readonly bool _allowEmptyValues;
        private readonly bool _allowDuplicatedValues;

        public GuidCollectionAttribute(bool allowEmptyCollection, bool allowEmptyValues, bool allowDuplicatedValues)
        {
            _allowEmptyCollection = allowEmptyCollection;
            _allowEmptyValues = allowEmptyValues;
            _allowDuplicatedValues = allowDuplicatedValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var collection = (ICollection<Guid>)value;

                if (!_allowEmptyCollection && !collection.Any())
                    return new ValidationResult($"The field {validationContext.DisplayName} cannot be empty.");

                if (collection.Any())
                {
                    if (!_allowEmptyValues)
                    {
                        if (collection.Any(x => x == new Guid() || x == Guid.Empty))
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