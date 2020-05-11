using System.Threading.Tasks;
using Riva.AnnouncementPreferences.Core.Enums;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public interface IAnnouncementUrlsSenderService
    {
        Task SendAnnouncementUrlsAsync(AnnouncementSendingFrequency announcementSendingFrequency);
    }
}