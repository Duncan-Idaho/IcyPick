using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyPick.Fetcher.Output
{
    public record Data(
        IReadOnlyList<Hero> Heroes,
        IReadOnlyList<Map> Maps);

    public record Hero(
        string Id,
        string Name,
        string Role,
        Uri GuideUri,
        HeroMapPreference MapPreference,
        HeroSynergiesAndCounter SynergiesAndCounter,
        Dictionary<string, TierRecommendation> ConditionsForGeneralTier,
        Dictionary<string, TierRecommendation> ConditionsForMasterTier,
        Dictionary<string, TierRecommendation> ConditionsForAramTier);

    public record TierRecommendation(string Tier, bool BanRecommended);

    public record HeroMapPreference(
        IReadOnlyList<string> StrongerMaps,
        IReadOnlyList<string> AverageMaps,
        IReadOnlyList<string> WeakerMaps,
        string Strategy)
    {
        public static HeroMapPreference FromModel(Models.HeroMapPreference model)
            => new(
                model.StrongerMaps.Select(map => map.Id).ToList(),
                model.AverageMaps.Select(map => map.Id).ToList(),
                model.WeakerMaps.Select(map => map.Id).ToList(),
                model.Strategy);
    }

    public record HeroSynergiesAndCounter(
        IReadOnlyList<string> SynergicHeroes,
        string SynergySource,
        IReadOnlyList<string> CounteringHeroes,
        string CounterSource)
    {
        public static HeroSynergiesAndCounter FromModel(Models.HeroSynergiesAndCounter model)
            => new(
                model.SynergicHeroes,
                model.SynergySource,
                model.CounteringHeroes,
                model.CounterSource);
    }

    public record Map(string Id, string Name)
    {
        public static Map FromModel(Models.Map model) 
            => new(model.Id, model.Name);
    }
}
