using System;

namespace IcyPick.Fetcher.Models
{
    public record HeroGuide(
        string Id,
        string Name,
        string Category,
        Uri GuideUri,
        Uri IconUri,
        
        HeroMapPreference MapPreference,
        HeroSynergiesAndCounter SynergiesAndCounter) : Hero(Id, Name, Category, GuideUri, IconUri), IEntityWithImage
    {
        public HeroGuide(Hero hero, HeroMapPreference mapPreference, HeroSynergiesAndCounter synergiesAndCounter)
            : this(hero.Id, hero.Name, hero.Category, hero.GuideUri, hero.IconUri, mapPreference, synergiesAndCounter)
        {
        }
    }
}