namespace PoeApiClient.Models
{
    public interface IAccount
    {
        string Name { get; set; }
        Realm Realm { get; set; }
        IChallenges Challenges {get; set;}
    }

    public class Account : IAccount
    {
        public string Name { get; set; }
        public Realm Realm { get; set; }
        public IChallenges Challenges { get; set; }
    }
}
