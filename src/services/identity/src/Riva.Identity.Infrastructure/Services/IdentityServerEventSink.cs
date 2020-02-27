using System;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Riva.BuildingBlocks.Core.Communications;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Repositories;

namespace Riva.Identity.Infrastructure.Services
{
    public class IdentityServerEventSink : IEventSink
    {
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommunicationBus _communicationBus;

        public IdentityServerEventSink(IAccountGetterService accountGetterService, IAccountRepository accountRepository, 
            ICommunicationBus communicationBus)
        {
            _accountGetterService = accountGetterService;
            _accountRepository = accountRepository;
            _communicationBus = communicationBus;
        }

        public async Task PersistAsync(Event evt)
        {
            if (evt is TokenIssuedSuccessEvent @event)
            {
                var getAccountResult = await _accountGetterService.GetByIdAsync(new Guid(@event.SubjectId));
                if (!getAccountResult.Success)
                    return;

                getAccountResult.Value.Login(Guid.NewGuid());
                await _communicationBus.DispatchDomainEventsAsync(getAccountResult.Value);
                await _accountRepository.UpdateAsync(getAccountResult.Value);
            }
        }
    }
}