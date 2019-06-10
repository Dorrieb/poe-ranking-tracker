using PoeApiClient.Models;
using System;
using System.Collections.Generic;

namespace PoeApiClient.Events
{
    public class HttpRequestEventArgs : EventArgs
    {
        public bool? Success { get; set; }
#pragma warning disable CA2227
        public List<IRuleApi> Rules { get; set; }
        public List<IRuleApi> RulesState { get; set; }
#pragma warning restore CA2227
    }
}
