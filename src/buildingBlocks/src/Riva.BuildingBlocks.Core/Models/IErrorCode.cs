namespace Riva.BuildingBlocks.Core.Models
{
    public interface IErrorCode
    {
        int Value { get; }
        string DisplayName { get; }
    }
}