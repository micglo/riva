using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentAssertions;
using Moq;
using Riva.Users.Core.Services;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class BlobContainerServiceTest
    {
        private readonly Mock<BlobContainerClient> _blobContainerClientMock;
        private readonly IBlobContainerService _service;

        public BlobContainerServiceTest()
        {
            _blobContainerClientMock = new Mock<BlobContainerClient>();
            _service = new BlobContainerService(_blobContainerClientMock.Object);
        }

        [Fact]
        public async Task UploadFileAsync_Should_Upload_File_To_Blob_Container()
        {
            const string fileName = "file123";
            const string blobUri = "https://accountName/container/" + fileName;
            var expectedResult = $"/container/{fileName}";
            var blobClientMock = new Mock<BlobClient>();
            var uploadRespMock = new Mock<Response<BlobContentInfo>>();
            var setMetadataRespMock = new Mock<Response<BlobInfo>>();
            blobClientMock.Setup(x => x.UploadAsync(It.IsAny<Stream>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uploadRespMock.Object);
            blobClientMock.Setup(x => x.SetMetadataAsync(It.IsAny<IDictionary<string, string>>(),
                It.IsAny<BlobRequestConditions>(), It.IsAny<CancellationToken>())).ReturnsAsync(setMetadataRespMock.Object);
            blobClientMock.SetupGet(x => x.Uri).Returns(new Uri(blobUri));

            _blobContainerClientMock.Setup(x => x.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);

            var result = await _service.UploadFileAsync(Array.Empty<byte>(), fileName, "image/jpg");

            result.Should().Be(expectedResult);
        }
    }
}