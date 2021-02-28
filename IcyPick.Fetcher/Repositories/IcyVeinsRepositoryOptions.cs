using System.ComponentModel.DataAnnotations;

namespace IcyPick.Fetcher.Repositories
{
    public class IcyVeinsRepositoryOptions
    {
        [Required]
        [Url]
        public string? BaseUrl { get; set; }
    }
}
