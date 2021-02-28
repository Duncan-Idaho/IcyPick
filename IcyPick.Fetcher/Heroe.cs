using System;

namespace IcyPick.Fetcher
{
    public record Heroe(
        string Name,
        string Category,
        Uri GuideUri,
        Uri IconUri);
}