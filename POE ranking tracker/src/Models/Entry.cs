using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeRankingTracker.Models
{
    public class Entry
    {
        public int Rank { set; get; }
        public bool Dead { set; get; }
        public bool Retired { set; get; }
        public bool Online { set; get; }
        public Character Character { set; get; }
        public Account Account { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType()) return false;

            Entry e = (Entry)obj;
            return (this.Character.Id == e.Character.Id);
        }

        public override int GetHashCode()
        {
            var hashCode = 584682452;
            hashCode = hashCode * -1425578 + Character.Id.GetHashCode();
            return hashCode;
        }
    }
}
