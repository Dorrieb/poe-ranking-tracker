using PoeApiClient.Models;
using System.Globalization;

namespace PoeRankingTracker.Models
{
    public class TrackerConfiguration
    {
        public ILeague League { get; set; }
        public IEntry Entry { get; set; }
        public string Template { get; set; }
        public string AccountName { get; set; }
        public CultureInfo Culture { get; set; }
        public bool InteractionsDisabled { get; set; }
    }
}
