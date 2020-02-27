namespace Riva.BuildingBlocks.WebApiTest.ValidationAttributeTests
{
    public class ValidationAttributeTestFixture
    {
        public ValidationAttributeTestExecutor TestExecutor { get; }

        public ValidationAttributeTestFixture()
        {
            TestExecutor = new ValidationAttributeTestExecutor();
        }
    }
}