namespace PoeRankingTracker.Models
{
    public class Character
    {
        public string Name { set; get; }
        public int Level { set; get; }
        public CharacterClass Class { set; get; }
        public string Id { set; get; }
        public long Experience { set; get; }
    }
}
