using System;
using System.ComponentModel.DataAnnotations;

namespace Riva.BuildingBlocks.WebApi.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredPropertyPairAttribute : ValidationAttribute
    {
        private readonly string _propertyName;

        public RequiredPropertyPairAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
                return ValidationResult.Success;

            var propertyValue = validationContext.ObjectType.GetProperty(_propertyName)
                ?.GetValue(validationContext.ObjectInstance);

            return propertyValue is null ? ValidationResult.Success : new ValidationResult($"The field {validationContext.DisplayName} is required.");
        }
    }
}