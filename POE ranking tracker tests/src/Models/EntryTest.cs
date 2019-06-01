using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker.Models;

namespace PoeRankingTrackerTests.Models
{
    [TestClass]
    public class EntryTest
    {

        private Account account;
        private readonly string accountName = "morinfa";
        private readonly Realm realm = Realm.pc;
        private readonly int total = 12;
        private Character character;
        private readonly string name = "Fragoulin";
        private readonly int level = 50;
        private readonly string id = "Fragoulin";
        private readonly CharacterClass characterClass = CharacterClass.Marauder;
        private readonly long experience = 10055248;
        private Entry entry;
        private readonly bool dead = false;
        private readonly bool online = false;
        private readonly bool retired = false;
        private readonly int rank = 50;

        [TestInitialize]
        public void TestSetup()
        {
            account = new Account
            {
                Name = accountName,
                Realm = realm,
                Challenges = new Challenges
                {
                    Total = total,
                },
            };
            character = new Character
            {
                Name = name,
                Level = level,
                Class = characterClass,
                Id = id,
                Experience = experience,
            };
            entry = new Entry
            {
                Account = account,
                Character = character,
                Dead = dead,
                Online = online,
                Rank = rank,
                Retired = retired,
            };
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(account, entry.Account);
            Assert.AreEqual(character, entry.Character);
            Assert.AreEqual(dead, entry.Dead);
            Assert.AreEqual(online, entry.Online);
            Assert.AreEqual(rank, entry.Rank);
            Assert.AreEqual(retired, entry.Retired);
        }

        [TestMethod]
        public void Set()
        {
            Account account2 = new Account
            {
                Name = $"{accountName}2",
                Realm = Realm.sony,
                Challenges = new Challenges
                {
                    Total = total + 2,
                },
            };
            Character character2 = new Character
            {
                Name = $"{name}2",
                Level = level + 2,
                Class = CharacterClass.Assassin,
                Id = $"{id}2",
                Experience = experience + 2,
            };
            entry.Account = account2;
            entry.Character = character2;
            entry.Dead = true;
            entry.Online = true;
            entry.Rank = 90;
            entry.Retired = true;
            Assert.AreEqual(account2, entry.Account);
            Assert.AreEqual(character2, entry.Character);
            Assert.AreEqual(true, entry.Dead);
            Assert.AreEqual(true, entry.Online);
            Assert.AreEqual(90, entry.Rank);
            Assert.AreEqual(true, entry.Retired);
        }

        [TestMethod]
        public void CheckEquals()
        {
            Character character2 = new Character
            {
                Name = name,
                Level = level,
                Class = characterClass,
                Id = id,
                Experience = experience,
            };
            Entry entry2 = new Entry {
                Account = account,
                Character = character2,
                Dead = dead,
                Online = online,
                Rank = rank,
                Retired = retired,
            };
            int hash1 = entry.GetHashCode();
            int hash2 = entry2.GetHashCode();
            Assert.AreEqual(hash1, hash2);
            Assert.IsTrue(entry2.Equals(entry));
            entry2.Character.Id = $"{id}2";
            hash2 = entry2.GetHashCode();
            Assert.IsFalse(entry2.Equals(entry));
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}