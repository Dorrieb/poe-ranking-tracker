using System.Collections.Generic;

namespace PoeApiClient.Models
{
    public interface ILadder
    {
#pragma warning disable CA2227
        List<IEntry> Entries { set;  get; }
#pragma warning restore CA2227
    }

    public class Ladder : ILadder
    {
#pragma warning disable CA2227
        public List<IEntry> Entries { set; get; }
#pragma warning restore CA2227
    }
}
