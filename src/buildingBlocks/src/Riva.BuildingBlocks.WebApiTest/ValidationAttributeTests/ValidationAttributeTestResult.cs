using System.Collections.Generic;

namespace Riva.BuildingBlocks.WebApiTest.ValidationAttributeTests
{
    public class ValidationAttributeTestResult
    {
        public bool IsValid { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}