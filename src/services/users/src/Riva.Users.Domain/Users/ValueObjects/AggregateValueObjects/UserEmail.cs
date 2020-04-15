using System;
using System.Net.Mail;
using Riva.Users.Domain.Users.Exceptions.AggregateExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.AggregateValueObjects
{
    public class UserEmail
    {
        private readonly string _email;
        private const int EmailMaxLength = 256;

        public UserEmail(string email)
        {
            CheckEmail(email);
            _email = email;
        }

        public static implicit operator string(UserEmail email)
        {
            return email._email;
        }

        private static void CheckEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new UserEmailNullException();

            try
            {
                var mailAddress = new MailAddress(email);
                if (mailAddress.Address.Length > EmailMaxLength)
                    throw new UserEmailMaxLengthException(EmailMaxLength);
            }
            catch (FormatException)
            {
                throw new UserEmailFormatException();
            }
            catch (ArgumentNullException)
            {
                throw new UserEmailNullException();
            }
            catch (ArgumentException)
            {
                throw new UserEmailNullException();
            }
        }
    }
}