using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyPick.Fetcher.Infrastructure
{
    public interface IMainService
    {
        public Task ExecuteAsync();
    }
}
