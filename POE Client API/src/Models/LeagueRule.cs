namespace PoeApiClient.Models
{
    public interface ILeagueRule
    {
        string Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }

    public class LeagueRule : ILeagueRule
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}