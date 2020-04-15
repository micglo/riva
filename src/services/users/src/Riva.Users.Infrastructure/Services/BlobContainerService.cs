using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Riva.Users.Core.Services;

namespace Riva.Users.Infrastructure.Services
{
    public class BlobContainerService : IBlobContainerService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobContainerService(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient;
        }

        public async Task<string> UploadFileAsync(byte[] fileData, string fileName, string fileContentType)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(new MemoryStream(fileData), true);
            var metadata = new Dictionary<string, string>
            {
                {
                    "ContentType",
                    fileContentType
                }
            };
            await blobClient.SetMetadataAsync(metadata);
            return blobClient.Uri.AbsolutePath;
        }
    }
}