using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeRankingTracker.Models
{
    public class Ladder
    {
        public string CachedSince { set; get; }
#pragma warning disable CA2227
        public List<Entry> Entries { set;  get; }
#pragma warning restore CA2227
    }
}
