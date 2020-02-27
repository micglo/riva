using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Riva.BuildingBlocks.WebApi.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowedValuesAttribute : ValidationAttribute
    {
        private readonly IEnumerable<string> _allowedValues;

        public AllowedValuesAttribute(params string[] allowedValues)
        {
            _allowedValues = allowedValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var allowedValues = string.Join(", ", _allowedValues);
            var errorMessage = $"The field {validationContext.DisplayName} has incorrect value. Allowed values are: {allowedValues}.";

            if (value != null)
            {
                if (value is ICollection)
                {
                    var collection = (ICollection<string>)value;

                    if (collection.Any() &&
                        !collection.Any(string.IsNullOrWhiteSpace) &&
                        collection.Select(x => x.ToLower()).Except(_allowedValues.Select(x => x.ToLower())).Any())
                        return new ValidationResult(errorMessage);
                }
                else
                {
                    var stringValue = value.ToString()?.ToLower();

                    if (!string.IsNullOrWhiteSpace(stringValue) &&
                        !_allowedValues.Select(x => x.ToLower()).Contains(stringValue))
                        return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}