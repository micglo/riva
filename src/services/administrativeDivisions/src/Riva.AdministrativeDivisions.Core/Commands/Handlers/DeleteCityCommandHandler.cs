using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;

namespace Riva.AdministrativeDivisions.Core.Commands.Handlers
{
    public class DeleteCityCommandHandler : ICommandHandler<DeleteCityCommand>
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICityGetterService _cityGetterService;

        public DeleteCityCommandHandler(ICityRepository cityRepository, ICityGetterService cityGetterService)
        {
            _cityRepository = cityRepository;
            _cityGetterService = cityGetterService;
        }

        public async Task HandleAsync(DeleteCityCommand command, CancellationToken cancellationToken = default)
        {
            var getCityResult = await _cityGetterService.GetByIdAsync(command.CityId);
            if(!getCityResult.Success)
                throw new ResourceNotFoundException(getCityResult.Errors);

            if(getCityResult.Value.RowVersion.Except(command.RowVersion).Any())
                throw new PreconditionFailedException();

            await _cityRepository.DeleteAsync(getCityResult.Value);
        }
    }
}