using System.Threading;
using System.Threading.Tasks;

namespace Riva.BuildingBlocks.Core.Queries
{
    public interface IQueryHandler<in TInput, TOutput> where TInput : IInputQuery where TOutput : IOutputQuery
    {
        Task<TOutput> HandleAsync(TInput inputQuery, CancellationToken cancellationToken = default);
    }
}