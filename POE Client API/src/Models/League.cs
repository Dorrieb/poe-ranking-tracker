using System;
using System.Collections.Generic;

namespace PoeApiClient.Models
{
    public interface ILeague
    {
        string Id { get; set; }
        Realm Realm { get; set; }
        string Description { get; set; }
        Uri Url { get; set; }
        DateTime StartAt { get; set; }
        DateTime? EndAt { get; set; }
        DateTime? RegisterAt { get; set; }
        bool DelveEvent { get; set; }
        bool LeagueEvent { get; set; }
        ILadder Ladder { get; set; }
#pragma warning disable CA2227
        List<ILeagueRule> Rules { get; set; }
#pragma warning restore CA2227
    }

    public class League : ILeague
    {
        public string Id { get; set; }
        public Realm Realm { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? RegisterAt { get; set; }
        public bool DelveEvent { get; set; }
        public bool LeagueEvent { get; set; }
        public ILadder Ladder { get; set; }
#pragma warning disable CA2227
        public List<ILeagueRule> Rules { get; set; }
#pragma warning restore CA2227
        public override string ToString()
        {
            return $"League : {Id}";
        }
    }
}