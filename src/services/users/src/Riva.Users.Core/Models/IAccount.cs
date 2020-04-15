using System;

namespace Riva.Users.Core.Models
{
    public interface IAccount
    {
        Guid Id { get; }
        string Email { get; }
    }
}