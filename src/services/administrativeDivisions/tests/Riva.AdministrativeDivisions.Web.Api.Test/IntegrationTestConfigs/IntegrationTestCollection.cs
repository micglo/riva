using Xunit;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs
{
    [CollectionDefinition("Integration tests collection")]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
    {

    }
}