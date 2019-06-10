using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Models;

namespace PoeApiClientTests.Models
{
    [TestClass]
    public class AccountTest
    {
        private Account account;
        private readonly string name = "morinfa";
        private readonly Realm realm = Realm.pc;
        private readonly int total = 12;

        [TestInitialize]
        public void TestSetup()
        {
            account = new Account {
                Name = name,
                Realm = realm,
                Challenges = new Challenges {
                    Total = total,
                },
            };
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(realm, account.Realm);
            Assert.AreEqual(total, account.Challenges.Total);
        }

        [TestMethod]
        public void Set()
        {
            string name2 = $"{name}2";
            Realm realm2 = Realm.sony;
            int total2 = total + 2;
            account.Name = name2;
            account.Realm = realm2;
            account.Challenges.Total = total2;
            Assert.AreEqual(name2, account.Name);
            Assert.AreEqual(realm2, account.Realm);
            Assert.AreEqual(total2, account.Challenges.Total);
        }
    }
}
