using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IcyPick.Fetcher
{
    public interface IHeroesRepository
    {
        Task<IReadOnlyList<Heroe>> GetHeroesAsync(CancellationToken cancellationToken = default);
    }
}