using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;

namespace Riva.AdministrativeDivisions.Core.Commands.Handlers
{
    public class DeleteCityDistrictCommandHandler : ICommandHandler<DeleteCityDistrictCommand>
    {
        private readonly ICityDistrictGetterService _cityDistrictGetterService;
        private readonly ICityDistrictRepository _cityDistrictRepository;

        public DeleteCityDistrictCommandHandler(ICityDistrictGetterService cityDistrictGetterService, 
            ICityDistrictRepository cityDistrictRepository)
        {
            _cityDistrictGetterService = cityDistrictGetterService;
            _cityDistrictRepository = cityDistrictRepository;
        }

        public async Task HandleAsync(DeleteCityDistrictCommand command, CancellationToken cancellationToken = default)
        {
            var getCityDistrictResult = await _cityDistrictGetterService.GetByIdAsync(command.CityDistrictId);
            if (!getCityDistrictResult.Success)
                throw new ResourceNotFoundException(getCityDistrictResult.Errors);

            if (getCityDistrictResult.Value.RowVersion.Except(command.RowVersion).Any())
                throw new PreconditionFailedException();

            await _cityDistrictRepository.DeleteAsync(getCityDistrictResult.Value);
        }
    }
}