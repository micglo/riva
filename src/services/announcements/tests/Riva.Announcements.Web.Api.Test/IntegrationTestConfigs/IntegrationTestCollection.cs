using Xunit;

namespace Riva.Announcements.Web.Api.Test.IntegrationTestConfigs
{
    [CollectionDefinition("Integration tests collection")]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
    {

    }
}