using Riva.AdministrativeDivisions.Domain.States.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.States.ValueObjects
{
    public class StateName
    {
        private const int MaxLength = 256;
        private readonly string _name;

        public StateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new StateNameNullException();
            if (name.Length > MaxLength)
                throw new StateNameMaxLengthException(MaxLength);

            _name = name;
        }

        public static implicit operator string(StateName name)
        {
            return name._name;
        }
    }
}