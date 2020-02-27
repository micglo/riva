using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.Identity.Web.Api.Models.Responses.Roles
{
    public class GetRoleResponse : VersionedResponseBase
    {
        public string Name { get; }
        public GetRoleResponse(Guid id, IEnumerable<byte> rowVersion, string name) : base(id, rowVersion)
        {
            Name = name;
        }
    }
}