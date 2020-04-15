using System;
using System.Linq;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Infrastructure.Services
{
    public class UserRevertService : IUserRevertService
    {
        private readonly IDomainEventStore _domainEventStore;
        private readonly IUserGetterService _userGetterService;
        private readonly IUserRepository _userRepository;

        public UserRevertService(IDomainEventStore domainEventStore, IUserGetterService userGetterService, IUserRepository userRepository)
        {
            _domainEventStore = domainEventStore;
            _userGetterService = userGetterService;
            _userRepository = userRepository;
        }

        public async Task RevertUserAsync(Guid userId, Guid correlationId)
        {
            var domainEvents = await _domainEventStore.FindAllAsync(userId);
            var domainEventsToApply = domainEvents.Where(x => x.CorrelationId != correlationId);
            var getUserResult = await _userGetterService.GetByIdAsync(userId);
            getUserResult.Value.AddEvents(domainEventsToApply);
            getUserResult.Value.ApplyEvents();
            await _userRepository.UpdateAsync(getUserResult.Value);
        }
    }
}