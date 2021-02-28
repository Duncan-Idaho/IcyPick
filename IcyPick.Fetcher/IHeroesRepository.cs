﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IcyPick.Fetcher
{
    public interface IHeroesRepository
    {
        Task<IReadOnlyList<Hero>> GetHeroesAsync(CancellationToken cancellationToken = default);
        Task<HeroGuide> GetHeroGuideAsync(Hero hero, CancellationToken cancellationToken);
    }
}