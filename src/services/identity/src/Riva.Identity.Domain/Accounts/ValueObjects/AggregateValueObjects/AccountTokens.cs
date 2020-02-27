using System.Collections.Generic;
using System.Linq;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.AggregateValueObjects
{
    public class AccountTokens
    {
        private readonly List<Token> _tokens;

        public AccountTokens(IEnumerable<Token> tokens)
        {
            if (tokens is null)
                throw new AccountTokensNullException();

            var tokenList = tokens.ToList();

            if (tokenList.Any(x => x is null))
                throw new AccountTokensInvalidException();

            var anyDuplicate = tokenList.GroupBy(x => x).Any(g => g.Count() > 1);
            if (anyDuplicate)
                throw new AccountTokensDuplicateValuesException();

            _tokens = new List<Token>(tokenList);
        }

        public static implicit operator List<Token>(AccountTokens tokens)
        {
            return tokens._tokens;
        }
    }
}