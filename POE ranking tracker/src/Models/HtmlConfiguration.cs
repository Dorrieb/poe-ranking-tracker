using PoeApiClient.Models;

namespace PoeRankingTracker.Models
{
    public class HtmlConfiguration
    {
        public CharacterClass CharacterClass { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public int RankByClass { get; set; }
        public long ExperienceAhead { get; set; }
        public long ExperienceBehind { get; set; }
        public long ExperiencePerHour { get; set; }
        public int DeadsAhead { get; set; }
    }
}
