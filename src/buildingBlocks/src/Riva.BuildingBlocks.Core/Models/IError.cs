namespace Riva.BuildingBlocks.Core.Models
{
    public interface IError
    {
        IErrorCode ErrorCode { get; }
        string ErrorMessage { get; }
    }
}