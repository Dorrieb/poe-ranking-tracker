namespace PoeApiClient.Models
{
    public interface IChallenges
    {
        int Total { get; set; }
    }

    public class Challenges : IChallenges
    {
        public int Total { get; set; }
    }
}
