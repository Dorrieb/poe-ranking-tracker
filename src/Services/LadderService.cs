using PoeRankingTracker.Exceptions;
using PoeRankingTracker.Models;
using System;

namespace PoeRankingTracker.Services
{
    public class LadderService
    {
        private static readonly Lazy<LadderService> lazy = new Lazy<LadderService>(() => new LadderService());
        public static LadderService Instance { get { return lazy.Value; } }

        public const int defaultRank = 15000;

        private LadderService()
        {

        }

        public int GetRank(Ladder ladder, string characterName)
        {
            var rank = defaultRank;

            if (ladder != null)
            {
                foreach (var entry in ladder.Entries)
                {
                    if (characterName == entry.Character.Name)
                    {
                        rank = entry.Rank;
                        break;
                    }
                }
            }

            return rank;
        }
    }
}
