using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Domain.Accounts.Enumerations;

namespace Riva.Identity.Infrastructure.Services
{
    public class AccountCreatorService : IAccountCreatorService
    {
        private readonly IPasswordService _passwordService;
        private readonly IRoleRepository _roleRepository;
        private readonly ICommunicationBus _communicationBus;
        private readonly IAccountRepository _accountRepository;

        public AccountCreatorService(IPasswordService passwordService, IRoleRepository roleRepository, 
            ICommunicationBus communicationBus, IAccountRepository accountRepository)
        {
            _passwordService = passwordService;
            _roleRepository = roleRepository;
            _communicationBus = communicationBus;
            _accountRepository = accountRepository;
        }

        public async Task<Account> CreateAsync(Guid id, string email, string password, Guid correlationId)
        {
            var passwordHash = string.Empty;
            var confirmed = true;

            if (!string.IsNullOrWhiteSpace(password))
            {
                passwordHash = _passwordService.HashPassword(password);
                confirmed = false;
            }
            var account = Account.Builder()
                .SetId(id)
                .SetEmail(email)
                .SetConfirmed(confirmed)
                .SetPasswordHash(passwordHash)
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            account.AddCreatedEvent(correlationId);

            if (!confirmed)
                account.GenerateToken(TokenTypeEnumeration.AccountConfirmation, correlationId);

            var role = await _roleRepository.GetByNameAsync(DefaultRoleEnumeration.User.DisplayName);
            account.AddRole(role.Id, correlationId);

            await _communicationBus.DispatchDomainEventsAsync(account);
            await _accountRepository.AddAsync(account);

            return account;
        }
    }
}