using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;

namespace Riva.Announcements.Core.Commands.Handlers
{
    public class DeleteRoomForRentAnnouncementCommandHandler : ICommandHandler<DeleteRoomForRentAnnouncementCommand>
    {
        private readonly IRoomForRentAnnouncementGetterService _roomForRentAnnouncementGetterService;
        private readonly IRoomForRentAnnouncementRepository _roomForRentAnnouncementRepository;

        public DeleteRoomForRentAnnouncementCommandHandler(IRoomForRentAnnouncementGetterService roomForRentAnnouncementGetterService, 
            IRoomForRentAnnouncementRepository roomForRentAnnouncementRepository)
        {
            _roomForRentAnnouncementGetterService = roomForRentAnnouncementGetterService;
            _roomForRentAnnouncementRepository = roomForRentAnnouncementRepository;
        }

        public async Task HandleAsync(DeleteRoomForRentAnnouncementCommand command, CancellationToken cancellationToken = default)
        {
            var getRoomForRentAnnouncementResult = await _roomForRentAnnouncementGetterService.GetByIdAsync(command.FlatForRentAnnouncementId);
            if (!getRoomForRentAnnouncementResult.Success)
                throw new ResourceNotFoundException(getRoomForRentAnnouncementResult.Errors);

            await _roomForRentAnnouncementRepository.DeleteAsync(getRoomForRentAnnouncementResult.Value);
        }
    }
}