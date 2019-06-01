using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PoeRankingTracker.Installers;
using PoeRankingTracker.Models;
using PoeRankingTracker.Services;
using System.Text;

namespace PoeRankingTrackerTests.Services
{
    [TestClass()]
    public class LadderServiceTests : BaseUnitTest
    {
        private ILadderService ladderService;
        private IWindsorContainer container;

        [TestInitialize]
        public void TestSetup()
        {
            container = new WindsorContainer().Install(new ServicesInstaller());

            ladderService = container.Resolve<ILadderService>();
        }

        [TestMethod]
        public void GetRankWithNullLadder()
        {
            int rank = ladderService.GetRank(null, "");
            Assert.AreEqual(LadderService.defaultRank, rank, "Incorrect default rank");
        }

        [TestMethod]
        public void GetRankWithLadderWithoutEntries()
        {
            string ladderJson = Encoding.UTF8.GetString(Properties.Resources.LadderNoEntries); ;
            var ladder = JsonConvert.DeserializeObject<Ladder>(ladderJson);
            int rank = ladderService.GetRank(ladder, "");
            Assert.AreEqual(LadderService.defaultRank, rank, "Incorrect default rank");
        }

        [TestMethod]
        public void GetRankWithLadderWithEntries()
        {
            string ladderJson = Encoding.UTF8.GetString(Properties.Resources.Ladder); ;
            var ladder = JsonConvert.DeserializeObject<Ladder>(ladderJson);
            int rank;
            rank = ladderService.GetRank(ladder, "zair_DICK_VAN_DYKE");
            Assert.AreEqual(1, rank, "Incorrect default rank for zair_DICK_VAN_DYKE");
            rank = ladderService.GetRank(ladder, "UberRankOneOrRiot");
            Assert.AreEqual(2, rank, "Incorrect default rank for UberRankOneOrRiot");
            rank = ladderService.GetRank(ladder, "Ethereal_KINO");
            Assert.AreEqual(3, rank, "Incorrect default rank for Ethereal_KINO");
            rank = ladderService.GetRank(ladder, "fghjbkghbn");
            Assert.AreEqual(4, rank, "Incorrect default rank for fghjbkghbn");
            rank = ladderService.GetRank(ladder, "Debilitating_autism");
            Assert.AreEqual(5, rank, "Incorrect default rank for Debilitating_autism");
            rank = ladderService.GetRank(ladder, "Reymeaea");
            Assert.AreEqual(6, rank, "Incorrect default rank for Reymeaea");
        }

        [TestCleanup]
        public void Cleanup()
        {
            container.Dispose();
        }
    }
}