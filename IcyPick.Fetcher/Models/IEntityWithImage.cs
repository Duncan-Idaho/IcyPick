using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyPick.Fetcher.Models
{
    public interface IEntityWithImage
    {
        string Id { get; }
        Uri IconUri { get; }
    }
}
