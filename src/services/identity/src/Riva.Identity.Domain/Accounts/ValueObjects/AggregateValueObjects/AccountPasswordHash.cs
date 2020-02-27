using Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.AggregateValueObjects
{
    public class AccountPasswordHash
    {
        private readonly string _passwordHash;

        public AccountPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new AccountPasswordHashNullException();

            _passwordHash = passwordHash;
        }

        public static implicit operator string(AccountPasswordHash passwordHash)
        {
            return passwordHash._passwordHash;
        }
    }
}