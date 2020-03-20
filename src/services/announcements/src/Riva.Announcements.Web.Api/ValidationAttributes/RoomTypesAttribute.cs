using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Riva.Announcements.Web.Api.Models.Enums;

namespace Riva.Announcements.Web.Api.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RoomTypesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var collection = (ICollection<RoomType>)value;

                if (collection.Any())
                {
                    var anyDuplicate = collection.GroupBy(x => x).Any(g => g.Count() > 1);
                    if (anyDuplicate)
                        return new ValidationResult($"The field {validationContext.DisplayName} cannot have duplicated values.");
                }
            }

            return ValidationResult.Success;
        }
    }
}