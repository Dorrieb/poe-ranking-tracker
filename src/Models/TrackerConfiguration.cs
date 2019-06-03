using PoeApiClient.Models;
using System.Drawing;
using System.Globalization;

namespace PoeRankingTracker.Models
{
    public class TrackerConfiguration
    {
        public ILeague League { get; set; }
        public IEntry Entry { get; set; }
        public Font Font { get; set; }
        public Color FontColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool ShowDeadsAhead { get; set; }
        public bool ShowExperienceAhead { get; set; }
        public bool ShowExperienceBehind { get; set; }
        public bool ShowProgressBar { get; set; }
        public string AccountName { get; set; }
        public CultureInfo Culture { get; set; }
    }
}
