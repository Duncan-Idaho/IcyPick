using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyPick.Fetcher.Models
{
    public record Tier(string Name, IReadOnlyList<Tier.Hero> Heroes)
    {
        public record Hero(string Id, bool BanRecommended, string? Condition);
    }
}
