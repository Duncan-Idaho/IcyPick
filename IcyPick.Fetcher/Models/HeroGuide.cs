using System;

namespace IcyPick.Fetcher.Models
{
    public record HeroGuide(
        string Id,
        string Name,
        string Role,
        Uri GuideUri,
        Uri IconUri,
        
        HeroMapPreference MapPreference,
        HeroSynergiesAndCounter SynergiesAndCounter) : Hero(Id, Name, Role, GuideUri, IconUri), IEntityWithImage
    {
        public HeroGuide(Hero hero, HeroMapPreference mapPreference, HeroSynergiesAndCounter synergiesAndCounter)
            : this(hero.Id, hero.Name, hero.Role, hero.GuideUri, hero.IconUri, mapPreference, synergiesAndCounter)
        {
        }
    }
}