using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeRankingTracker.Models
{
    public class Account
    {
        public string Name { get; set; }
        public string Realm { get; set; }
        public Challenges Challenges {get; set;}
    }
}
