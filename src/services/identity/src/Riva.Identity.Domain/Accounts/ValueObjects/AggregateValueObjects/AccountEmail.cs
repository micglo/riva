using System;
using System.Net.Mail;
using Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.AggregateValueObjects
{
    public class AccountEmail
    {
        private readonly string _email;
        private const int EmailMaxLength = 256;

        public AccountEmail(string email)
        {
            CheckEmail(email);
            _email = email;
        }

        public static implicit operator string(AccountEmail email)
        {
            return email._email;
        }

        private static void CheckEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                throw new AccountEmailNullException();

            try
            {
                var mailAddress = new MailAddress(email);
                if (mailAddress.Address.Length > EmailMaxLength)
                    throw new AccountEmailMaxLengthException(EmailMaxLength);
            }
            catch (FormatException)
            {
                throw new AccountEmailFormatException();
            }
            catch (ArgumentNullException)
            {
                throw new AccountEmailNullException();
            }
            catch (ArgumentException)
            {
                throw new AccountEmailNullException();
            }
        }
    }
}