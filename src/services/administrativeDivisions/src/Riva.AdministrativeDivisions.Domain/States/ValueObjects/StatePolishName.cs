using Riva.AdministrativeDivisions.Domain.States.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.States.ValueObjects
{
    public class StatePolishName
    {
        private const int MaxLength = 256;
        private readonly string _polishName;

        public StatePolishName(string polishName)
        {
            if (string.IsNullOrWhiteSpace(polishName))
                throw new StatePolishNameNullException();
            if (polishName.Length > MaxLength)
                throw new StatePolishNameMaxLengthException(MaxLength);

            _polishName = polishName;
        }

        public static implicit operator string(StatePolishName polishName)
        {
            return polishName._polishName;
        }
    }
}