using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTestConfigs
{
    [CollectionDefinition("Integration tests collection")]
    public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
    {

    }
}