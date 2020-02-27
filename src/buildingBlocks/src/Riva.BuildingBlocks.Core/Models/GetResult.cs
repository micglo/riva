using System.Collections.Generic;
using System.Linq;

namespace Riva.BuildingBlocks.Core.Models
{
    public class GetResult<T> where T : class
    {
        public bool Success { get; }
        public T Value { get; }
        public IReadOnlyCollection<IError> Errors { get; }

        private GetResult(bool success, T value, IEnumerable<IError> errors)
        {
            Success = success;
            Errors = errors.ToList().AsReadOnly();
            Value = value;
        }

        public static GetResult<T> Ok(T value)
        {
            return new GetResult<T>(true, value, new List<IError>());
        }

        public static GetResult<T> Fail(IEnumerable<IError> errors)
        {
            return new GetResult<T>(false, null, errors);
        }
    }
}