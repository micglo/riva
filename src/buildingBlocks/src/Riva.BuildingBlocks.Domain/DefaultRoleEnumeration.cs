namespace Riva.BuildingBlocks.Domain
{
    public class DefaultRoleEnumeration : EnumerationBase
    {
        public static DefaultRoleEnumeration Administrator => new DefaultRoleEnumeration(1, nameof(Administrator));
        public static DefaultRoleEnumeration User => new DefaultRoleEnumeration(2, nameof(User));
        public static DefaultRoleEnumeration System => new DefaultRoleEnumeration(3, nameof(System));

        private DefaultRoleEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}