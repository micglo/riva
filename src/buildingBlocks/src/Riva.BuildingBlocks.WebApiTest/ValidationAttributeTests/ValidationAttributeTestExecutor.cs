using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Riva.BuildingBlocks.WebApiTest.ValidationAttributeTests
{
    public class ValidationAttributeTestExecutor
    {
        public ValidationAttributeTestResult Execute(object validationTarget)
        {
            var context = new ValidationContext(validationTarget);
            var validationResults = new Collection<ValidationResult>();
            var isValid = Validator.TryValidateObject(validationTarget, context, validationResults, true);
            var errorMsg = validationResults.Select(x => x.ErrorMessage);

            return new ValidationAttributeTestResult
            {
                IsValid = isValid,
                Errors = errorMsg
            };
        }
    }
}