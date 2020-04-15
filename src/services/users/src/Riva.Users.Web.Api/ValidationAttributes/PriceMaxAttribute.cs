using System;
using System.ComponentModel.DataAnnotations;

namespace Riva.Users.Web.Api.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PriceMaxAttribute : ValidationAttribute
    {
        private const string PriceMinPropertyName = "PriceMin";
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            var priceMinProperty = validationContext.ObjectType.GetProperty(PriceMinPropertyName)?.GetValue(validationContext.ObjectInstance);

            if (priceMinProperty is null)
                return ValidationResult.Success;

            var priceMax = (decimal) value;
            var priceMin = (decimal) priceMinProperty;

            return priceMax < priceMin 
                ? new ValidationResult($"The field {validationContext.DisplayName} must me greater or equal to {PriceMinPropertyName} value.") 
                : ValidationResult.Success;
        }
    }
}