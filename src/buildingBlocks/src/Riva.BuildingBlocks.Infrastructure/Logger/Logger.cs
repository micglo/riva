using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;

namespace Riva.BuildingBlocks.Infrastructure.Logger
{
    public class Logger : Core.Logger.ILogger
    {
        private readonly ILogger _logger;

        public Logger(ILogger<Logger> logger)
        {
            _logger = logger;
        }

        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, message, args);
        }

        public void LogError(ServiceComponentEnumeration serviceComponent, string message, params object[] args)
        {
            var errorMessage = "serviceComponent={serviceComponent}, " + message;
            args = args.Prepend(serviceComponent.DisplayName).ToArray();
            _logger.LogError(errorMessage, args);
        }

        public void LogIntegrationEventError(ServiceComponentEnumeration serviceComponent, IIntegrationEvent integrationEvent, string message,
            params object[] args)
        {
            var errorMessage = "serviceComponent={serviceComponent}, integrationEvent={integrationEvent}, correlationId={correlationId}, " + message;
            args = args.Prepend(integrationEvent.CorrelationId).Prepend(integrationEvent.GetType().Name).Prepend(serviceComponent.DisplayName).ToArray();
            _logger.LogError(errorMessage, args);
        }
    }
}