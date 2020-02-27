using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;

namespace Riva.BuildingBlocks.Core.Logger
{
    public interface ILogger
    {
        void LogError(string message, params object[] args);

        void LogError(Exception ex, string message, params object[] args);

        void LogError(ServiceComponentEnumeration serviceComponent, string message, params object[] args);

        void LogIntegrationEventError(ServiceComponentEnumeration serviceComponent, IIntegrationEvent integrationEvent,
            string message, params object[] args);
    }
}