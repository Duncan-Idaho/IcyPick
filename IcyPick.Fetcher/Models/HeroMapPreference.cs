using System.Collections.Generic;

namespace IcyPick.Fetcher.Models
{
    public record HeroMapPreference(
        IReadOnlyList<string> StrongerMaps,
        IReadOnlyList<string> AverageMaps,
        IReadOnlyList<string> WeakerMaps,
        string Strategy);
}