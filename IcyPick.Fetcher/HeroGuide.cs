using System;

namespace IcyPick.Fetcher
{
    public record HeroGuide(
        string Id,
        string Name,
        string Category,
        Uri GuideUri,
        Uri IconUri,
        
        HeroMapPreference Preference,
        HeroSynergiesAndCounter SynergiesAndCounter) : Hero(Id, Name, Category, GuideUri, IconUri)
    {
        public HeroGuide(Hero hero, HeroMapPreference preference, HeroSynergiesAndCounter synergiesAndCounter)
            : this(hero.Id, hero.Name, hero.Category, hero.GuideUri, hero.IconUri, preference, synergiesAndCounter)
        {
        }
    }
}