using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.Services
{
    public class StateVerificationService : IStateVerificationService
    {
        private readonly IStateRepository _stateRepository;

        public StateVerificationService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<VerificationResult> VerifyNameIsNotTakenAsync(string name)
        {
            return await IsNameAlreadyUsedAsync(name)
                ? VerificationResult.Fail(new Collection<IError> { new Error(StateErrorCodeEnumeration.NameAlreadyInUse, StateErrorMessage.NameAlreadyInUse) })
                : VerificationResult.Ok();
        }

        public async Task<VerificationResult> VerifyPolishNameIsNotTakenAsync(string polishName)
        {
            return await IsPolishNameAlreadyUsedAsync(polishName)
                ? VerificationResult.Fail(new Collection<IError>
                    { new Error(StateErrorCodeEnumeration.PolishNameAlreadyInUse, StateErrorMessage.PolishNameAlreadyInUse) })
                : VerificationResult.Ok();
        }

        private async Task<bool> IsNameAlreadyUsedAsync(string name)
        {
            return await _stateRepository.GetByNameAsync(name) != null;
        }

        private async Task<bool> IsPolishNameAlreadyUsedAsync(string polishName)
        {
            return await _stateRepository.GetByPolishNameAsync(polishName) != null;
        }
    }
}