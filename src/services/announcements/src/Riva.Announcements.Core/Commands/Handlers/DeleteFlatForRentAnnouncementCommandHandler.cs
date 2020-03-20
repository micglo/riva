using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;

namespace Riva.Announcements.Core.Commands.Handlers
{
    public class DeleteFlatForRentAnnouncementCommandHandler : ICommandHandler<DeleteFlatForRentAnnouncementCommand>
    {
        private readonly IFlatForRentAnnouncementGetterService _flatForRentAnnouncementGetterService;
        private readonly IFlatForRentAnnouncementRepository _flatForRentAnnouncementRepository;

        public DeleteFlatForRentAnnouncementCommandHandler(IFlatForRentAnnouncementGetterService flatForRentAnnouncementGetterService, 
            IFlatForRentAnnouncementRepository flatForRentAnnouncementRepository)
        {
            _flatForRentAnnouncementGetterService = flatForRentAnnouncementGetterService;
            _flatForRentAnnouncementRepository = flatForRentAnnouncementRepository;
        }

        public async Task HandleAsync(DeleteFlatForRentAnnouncementCommand command, CancellationToken cancellationToken = default)
        {
            var getFlatForRentAnnouncementResult = await _flatForRentAnnouncementGetterService.GetByIdAsync(command.FlatForRentAnnouncementId);
            if (!getFlatForRentAnnouncementResult.Success)
                throw new ResourceNotFoundException(getFlatForRentAnnouncementResult.Errors);

            await _flatForRentAnnouncementRepository.DeleteAsync(getFlatForRentAnnouncementResult.Value);
        }
    }
}