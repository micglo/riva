using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.Identity.Core.Models;

namespace Riva.Identity.Core.Services
{
    public interface ISchemeService
    {
        Task<IEnumerable<AuthenticationScheme>> GetAllSchemesAsync();
        Task<AuthenticationScheme> GetSchemeAsync(string name);
        Task<bool> SchemeSupportsSignOutAsync(string scheme);
    }
}