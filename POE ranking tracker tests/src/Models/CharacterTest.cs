using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker.Models;

namespace PoeRankingTrackerTests.Models
{
    [TestClass]
    public class CharacterTest
    {
        private Character character;
        private readonly string name = "Fragoulin";
        private readonly int level = 50;
        private readonly string id = "Fragoulin";
        private readonly CharacterClass characterClass = CharacterClass.Marauder;
        private readonly long experience = 10055248;
        
        [TestInitialize]
        public void TestSetup()
        {
            character = new Character
            {
                Name = name,
                Level = level,
                Class = characterClass,
                Id = id,
                Experience = experience,
            };
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(name, character.Name);
            Assert.AreEqual(level, character.Level);
            Assert.AreEqual(characterClass, character.Class);
            Assert.AreEqual(id, character.Id);
            Assert.AreEqual(experience, character.Experience);
        }

        [TestMethod]
        public void Set()
        {
            string name2 = $"{name}2";
            int level2 = level + 2;
            string id2 = $"{id}2";
            CharacterClass characterClass2 = CharacterClass.Necromancer;
            long experience2 = experience + 5620;
            character.Name = name2;
            character.Level = level2;
            character.Id = id2;
            character.Class = characterClass2;
            character.Experience = experience2;
            Assert.AreEqual(name2, character.Name);
            Assert.AreEqual(level2, character.Level);
            Assert.AreEqual(id2, character.Id);
            Assert.AreEqual(characterClass2, character.Class);
            Assert.AreEqual(experience2, character.Experience);
        }
    }
}
