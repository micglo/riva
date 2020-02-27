using System;
using System.Threading.Tasks;
using Riva.Identity.Domain.Clients.Aggregates;

namespace Riva.Identity.Domain.Clients.Repositories
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(Guid id);
    }
}