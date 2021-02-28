using IcyPick.Fetcher.Infrastructure;
using IcyPick.Fetcher.Models;
using IcyPick.Fetcher.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace IcyPick.Fetcher
{
    public class HeroesFetchingOperationOptions
    {
        public string DataOut { get; set; } = string.Empty;
        public string ImagesOutDir { get; set; } = string.Empty;
    }
}
