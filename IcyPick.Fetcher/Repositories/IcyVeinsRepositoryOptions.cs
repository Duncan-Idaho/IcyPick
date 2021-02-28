using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IcyPick.Fetcher.Repositories
{
    public class IcyVeinsRepositoryOptions
    {
        [Required]
        [Url]
        public string? BaseUrl { get; set; }
    }
}
