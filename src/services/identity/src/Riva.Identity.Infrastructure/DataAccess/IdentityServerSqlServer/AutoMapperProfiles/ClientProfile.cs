using System;
using AutoMapper;
using Riva.Identity.Domain.Clients.Aggregates;

namespace Riva.Identity.Infrastructure.DataAccess.IdentityServerSqlServer.AutoMapperProfiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<IdentityServer4.Models.Client, Client>()
                .ConstructUsing(
                    x => Client.Builder()
                        .SetId(new Guid(x.ClientId))
                        .SetEnabled(x.Enabled)
                        .SetEnableLocalLogin(x.EnableLocalLogin)
                        .SetRequirePkce(x.RequirePkce)
                        .Build()
                 );
        }
    }
}