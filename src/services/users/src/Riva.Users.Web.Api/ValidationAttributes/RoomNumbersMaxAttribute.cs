using System;
using System.ComponentModel.DataAnnotations;

namespace Riva.Users.Web.Api.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RoomNumbersMaxAttribute : ValidationAttribute
    {
        private const string RoomNumbersMinPropertyName = "RoomNumbersMin";
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            var roomNumbersMinProperty = validationContext.ObjectType.GetProperty(RoomNumbersMinPropertyName)?.GetValue(validationContext.ObjectInstance);

            if (roomNumbersMinProperty is null)
                return ValidationResult.Success;

            var roomNumbersMax = (int)value;
            var roomNumbersMin = (int)roomNumbersMinProperty;

            return roomNumbersMax < roomNumbersMin
                ? new ValidationResult($"The field {validationContext.DisplayName} must me greater or equal to {RoomNumbersMinPropertyName} value.")
                : ValidationResult.Success;
        }
    }
}