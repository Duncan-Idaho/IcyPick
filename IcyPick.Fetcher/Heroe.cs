using System;

namespace IcyPick.Fetcher
{
    public record Heroe(
        string Id,
        string Name,
        string Category,
        Uri GuideUri,
        Uri IconUri);
}