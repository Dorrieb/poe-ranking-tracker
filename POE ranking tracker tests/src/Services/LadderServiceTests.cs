using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PoeRankingTracker.Models;
using PoeRankingTracker.Services;
using System.Text;

namespace PoeRankingTrackerTests.Services
{
    [TestClass()]
    public class LadderServiceTests
    {
        [TestMethod]
        public void GetRankWithNullLadder()
        {
            int rank = LadderService.Instance.GetRank(null, "");
            Assert.AreEqual(LadderService.defaultRank, rank, "Incorrect default rank");
        }

        [TestMethod]
        public void GetRankWithLadderWithoutEntries()
        {
            string ladderJson = Encoding.UTF8.GetString(Properties.Resources.LadderNoEntries); ;
            var ladder = JsonConvert.DeserializeObject<Ladder>(ladderJson);
            int rank = LadderService.Instance.GetRank(ladder, "");
            Assert.AreEqual(LadderService.defaultRank, rank, "Incorrect default rank");
        }

        [TestMethod]
        public void GetRankWithLadderWithEntries()
        {
            string ladderJson = Encoding.UTF8.GetString(Properties.Resources.Ladder); ;
            var ladder = JsonConvert.DeserializeObject<Ladder>(ladderJson);
            int rank;
            rank = LadderService.Instance.GetRank(ladder, "zair_DICK_VAN_DYKE");
            Assert.AreEqual(1, rank, "Incorrect default rank for zair_DICK_VAN_DYKE");
            rank = LadderService.Instance.GetRank(ladder, "UberRankOneOrRiot");
            Assert.AreEqual(2, rank, "Incorrect default rank for UberRankOneOrRiot");
            rank = LadderService.Instance.GetRank(ladder, "Ethereal_KINO");
            Assert.AreEqual(3, rank, "Incorrect default rank for Ethereal_KINO");
            rank = LadderService.Instance.GetRank(ladder, "fghjbkghbn");
            Assert.AreEqual(4, rank, "Incorrect default rank for fghjbkghbn");
            rank = LadderService.Instance.GetRank(ladder, "Debilitating_autism");
            Assert.AreEqual(5, rank, "Incorrect default rank for Debilitating_autism");
            rank = LadderService.Instance.GetRank(ladder, "Reymeaea");
            Assert.AreEqual(6, rank, "Incorrect default rank for Reymeaea");
        }
    }
}