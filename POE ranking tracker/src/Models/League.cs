using System;
using System.Collections.Generic;

namespace PoeRankingTracker.Models
{
    public class League
    {
        public string Id { get; set; }
        public string Realm { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime RegisterAt { get; set; }
        public bool DelveEvent { get; set; }
        public bool Event { get; set; }
#pragma warning disable CA2227
        public List<LeagueRule> Rules { get; set; }
#pragma warning restore CA2227
        public override string ToString()
        {
            return $"League : {Id}";
        }
    }
}