using Newtonsoft.Json;

namespace PoeApiClient.Models
{
    public interface ICharacter
    {
        string Name { set; get; }
        int Level { set; get; }
        CharacterClass CharacterClass { set; get; }
        string Id { set; get; }
        long Experience { set; get; }
    }

    public class Character : ICharacter
    {
        public string Name { set; get; }
        public int Level { set; get; }
        [JsonProperty("Class")]
        public CharacterClass CharacterClass { set; get; }
        public string Id { set; get; }
        public long Experience { set; get; }
    }
}
