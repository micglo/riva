namespace Riva.Users.Core.Models
{
    public class PictureDto
    {
        public byte[] Data { get; }
        public string ContentType { get; }

        public PictureDto(byte[] data, string contentType)
        {
            Data = data;
            ContentType = contentType;
        }
    }
}