using System.Threading.Tasks;

namespace Riva.Users.Core.Services
{
    public interface IBlobContainerService
    {
        Task<string> UploadFileAsync(byte[] fileData, string fileName, string fileContentType);
    }
}