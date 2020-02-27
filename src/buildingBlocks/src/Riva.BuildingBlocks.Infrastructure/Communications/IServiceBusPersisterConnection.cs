using Microsoft.Azure.ServiceBus;

namespace Riva.BuildingBlocks.Infrastructure.Communications
{
    public interface IServiceBusPersisterConnection
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }
        ITopicClient CreateTopicClient();
    }
}