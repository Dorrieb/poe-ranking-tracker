using System;

namespace PoeRankingTracker.Events
{
    public class ApiEventArgs : EventArgs
    {
        public int Value { get; set; }
    }
}
