using System;
using System.Collections.Generic;

namespace IcyPick.Fetcher
{
    public record HeroSynergiesAndCounter(
        IReadOnlyList<string> SynergicHeroes,
        string SynergySource,
        IReadOnlyList<string> CounteringHeroes,
        string CounterSource);
}