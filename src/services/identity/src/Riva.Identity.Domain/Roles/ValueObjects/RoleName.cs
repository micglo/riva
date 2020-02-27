using Riva.Identity.Domain.Roles.Exceptions;

namespace Riva.Identity.Domain.Roles.ValueObjects
{
    public class RoleName
    {
        private const int MaxLength = 256;
        private readonly string _name;

        public RoleName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new RoleNameNullException();
            if (name.Length > MaxLength)
                throw new RoleNameMaxLengthException(MaxLength);

            _name = name;
        }

        public static implicit operator string(RoleName name)
        {
            return name._name;
        }
    }
}