using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IcyPick.Fetcher
{
    public interface IHeroesRepository
    {
        Task<HeroeGuide> GetHeroeGuideAsync(Heroe heroe, CancellationToken cancellationToken);
        Task<IReadOnlyList<Heroe>> GetHeroesAsync(CancellationToken cancellationToken = default);
    }
}