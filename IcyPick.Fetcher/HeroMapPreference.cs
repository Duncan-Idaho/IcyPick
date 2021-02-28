using System;
using System.Collections.Generic;

namespace IcyPick.Fetcher
{
    public record HeroMapPreference(
        IReadOnlyList<string> StrongerMaps,
        IReadOnlyList<string> AverageMaps,
        IReadOnlyList<string> WeakerMaps,
        string Strategy);
}