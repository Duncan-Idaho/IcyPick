using System;

namespace IcyPick.Fetcher.Models
{
    public record Hero(
        string Id,
        string Name,
        string Role,
        Uri GuideUri,
        Uri IconUri);
}