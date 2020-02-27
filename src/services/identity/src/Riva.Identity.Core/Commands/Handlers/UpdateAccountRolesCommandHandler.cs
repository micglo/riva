using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.Identity.Domain.Roles.Repositories;

namespace Riva.Identity.Core.Commands.Handlers
{
    public class UpdateAccountRolesCommandHandler : ICommandHandler<UpdateAccountRolesCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountGetterService _accountGetterService;
        private readonly ICommunicationBus _communicationBus;

        public UpdateAccountRolesCommandHandler(IAccountRepository accountRepository, IRoleRepository roleRepository, 
            IAccountGetterService accountGetterService, ICommunicationBus communicationBus)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _accountGetterService = accountGetterService;
            _communicationBus = communicationBus;
        }

        public async Task HandleAsync(UpdateAccountRolesCommand command, CancellationToken cancellationToken = default)
        {
            var getAccountResult = await _accountGetterService.GetByIdAsync(command.AccountId);
            if (!getAccountResult.Success)
                throw new ResourceNotFoundException(getAccountResult.Errors);

            var rolesToRemove = getAccountResult.Value.Roles.Except(command.Roles).ToList();
            var rolesToAdd = command.Roles.Except(getAccountResult.Value.Roles).ToList();

            var userRole = await _roleRepository.GetByNameAsync(DefaultRoleEnumeration.User.DisplayName);

            if (!command.Roles.Contains(userRole.Id))
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.UserRoleIsNotRemovable, AccountErrorMessage.UserRoleIsNotRemovable)
                };
                throw new ValidationException(errors);
            }

            var rolesToCheck = await _roleRepository.FindByIdsAsync(command.Roles);
            var incorrectRoles = command.Roles.Except(rolesToCheck.Select(x => x.Id));
            if (incorrectRoles.Any())
            {
                var errors = new Collection<IError>
                {
                    new Error(RoleErrorCodeEnumeration.RolesNotFound, RoleErrorMessage.RolesNotFound)
                };
                throw new ValidationException(errors);
            }

            var correlationId = Guid.NewGuid();

            foreach (var role in rolesToRemove)
            {
                getAccountResult.Value.RemoveRole(role, correlationId);
            }

            foreach (var role in rolesToAdd)
            {
                getAccountResult.Value.AddRole(role, correlationId);
            }

            await _communicationBus.DispatchDomainEventsAsync(getAccountResult.Value, cancellationToken);
            await _accountRepository.UpdateAsync(getAccountResult.Value);
        }
    }
}