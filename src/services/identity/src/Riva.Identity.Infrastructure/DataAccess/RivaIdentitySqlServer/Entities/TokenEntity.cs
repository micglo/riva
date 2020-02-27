using System;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Enums;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities
{
    public class TokenEntity : EntityBase
    {
        public DateTimeOffset Issued { get; set; }
        public DateTimeOffset Expires { get; set; }
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public Guid AccountId { get; set; }
        public AccountEntity Account { get; set; }
    }
}