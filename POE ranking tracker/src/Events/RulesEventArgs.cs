using PoeRankingTracker.Models;
using System;
using System.Collections.Generic;

namespace PoeRankingTracker.Events
{
    public class RulesEventArgs : EventArgs
    {
#pragma warning disable CA2227
        public List<RuleApi> Rules { get; set; }
        public List<RuleApi> RulesState { get; set; }
#pragma warning restore CA2227
    }
}
