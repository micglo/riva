using System;
using System.ComponentModel.DataAnnotations;

namespace Riva.BuildingBlocks.WebApi.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PageAttribute : ValidationAttribute
    {
        private readonly string _pageSizePropertyName;

        public PageAttribute(string pageSizePropertyName)
        {
            _pageSizePropertyName = pageSizePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            if((int)value <= 0)
                return ValidationResult.Success;

            var pageSizeProperty = validationContext.ObjectType.GetProperty(_pageSizePropertyName)
                ?.GetValue(validationContext.ObjectInstance);

            if (pageSizeProperty is null)
                return ValidationResult.Success;

            var page = (int)value;
            var pageSize = (int)pageSizeProperty;

            var calculation = pageSize * (page - 1);
            return calculation >= 0 ? ValidationResult.Success : new ValidationResult($"The field {validationContext.DisplayName} has to big value."); 
        }
    }
}