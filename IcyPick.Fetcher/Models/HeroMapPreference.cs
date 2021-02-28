using System.Collections.Generic;
using System.Linq;

namespace IcyPick.Fetcher.Models
{
    public record HeroMapPreference(
        IReadOnlyList<Map> StrongerMaps,
        IReadOnlyList<Map> AverageMaps,
        IReadOnlyList<Map> WeakerMaps,
        string Strategy)
    {
        public IEnumerable<Map> AllMaps 
            => StrongerMaps.Concat(AverageMaps).Concat(WeakerMaps);
    }
}