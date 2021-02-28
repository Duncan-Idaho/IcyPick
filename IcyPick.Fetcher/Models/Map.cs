using System;

namespace IcyPick.Fetcher.Models
{
    public record Map(
        string Id,
        string Name,
        Uri IconUri) : IEntityWithImage;
}