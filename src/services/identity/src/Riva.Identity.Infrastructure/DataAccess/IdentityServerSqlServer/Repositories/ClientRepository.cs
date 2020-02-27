using System;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Domain.Clients.Aggregates;
using Riva.Identity.Domain.Clients.Repositories;

namespace Riva.Identity.Infrastructure.DataAccess.IdentityServerSqlServer.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IClientStore _clientStore;
        private readonly IMapper _mapper;

        public ClientRepository(IClientStore clientStore, IMapper mapper)
        {
            _clientStore = clientStore;
            _mapper = mapper;
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            var entity = await _clientStore.FindClientByIdAsync(id.ToString());
            return entity != null ? _mapper.Map<IdentityServer4.Models.Client, Client>(entity) : null;
        }
    }
}