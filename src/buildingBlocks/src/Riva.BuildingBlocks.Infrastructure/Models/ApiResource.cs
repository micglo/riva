namespace Riva.BuildingBlocks.Infrastructure.Models
{
    public class ApiResource
    {
        public string Name { get; }
        public string DisplayName { get; }

        public ApiResource(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}