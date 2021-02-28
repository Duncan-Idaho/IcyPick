using IcyPick.Fetcher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IcyPick.Fetcher.Repositories
{
    public interface IHeroesRepository
    {
        Task<IReadOnlyList<Hero>> GetHeroesAsync(CancellationToken cancellationToken = default);
        Task<HeroGuide> GetHeroGuideAsync(Hero hero, CancellationToken cancellationToken);
        Task DownloadImageAsync(IEntityWithImage entity, Func<Task<Stream>> outputStreamAsyncFactory, CancellationToken cancellationToken);
    }
}