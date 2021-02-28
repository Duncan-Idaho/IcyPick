using System;
using System.Collections.Generic;

namespace IcyPick.Fetcher
{
    public record HeroeSynergiesAndCounter(
        IReadOnlyList<string> SynergicHeroes,
        string SynergySource,
        IReadOnlyList<string> CounteringHeroes,
        string CounterSource);
}