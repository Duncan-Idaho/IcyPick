using System;

namespace IcyPick.Fetcher
{
    public record HeroeGuide(
        string Id,
        string Name,
        string Category,
        Uri GuideUri,
        Uri IconUri,
        
        HeroeMapPreference Preference,
        HeroeSynergiesAndCounter SynergiesAndCounter) : Heroe(Id, Name, Category, GuideUri, IconUri)
    {
        public HeroeGuide(Heroe heroe, HeroeMapPreference preference, HeroeSynergiesAndCounter synergiesAndCounter)
            : this(heroe.Id, heroe.Name, heroe.Category, heroe.GuideUri, heroe.IconUri, preference, synergiesAndCounter)
        {
        }
    }
}