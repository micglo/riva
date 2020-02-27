using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;

namespace Riva.Identity.Infrastructure.Services
{
    public class AccountProviderService : IAccountProviderService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountCreatorService _accountCreatorService;
        private readonly ICommunicationBus _communicationBus;

        public AccountProviderService(IAccountRepository accountRepository, IAccountGetterService accountGetterService, 
            IAccountCreatorService accountCreatorService, ICommunicationBus communicationBus)
        {
            _accountRepository = accountRepository;
            _accountGetterService = accountGetterService;
            _accountCreatorService = accountCreatorService;
            _communicationBus = communicationBus;
        }

        public async Task<Account> ProvideAccountForExternalLoginAsync(string email, Guid correlationId)
        {
            var getAccountResult = await _accountGetterService.GetByEmailAsync(email);
            Account account;
            if (!getAccountResult.Success)
            {
                account = await _accountCreatorService.CreateAsync(Guid.NewGuid(), email, string.Empty, correlationId);
            }
            else
            {
                account = getAccountResult.Value;
                if (account.Confirmed) 
                    return account;

                account.Confirm(correlationId);
                await _communicationBus.DispatchDomainEventsAsync(account);
                await _accountRepository.UpdateAsync(account);
            }

            return account;
        }
    }
}