using System;

namespace IcyPick.Fetcher
{
    public record Hero(
        string Id,
        string Name,
        string Category,
        Uri GuideUri,
        Uri IconUri);
}