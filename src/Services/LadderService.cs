using PoeRankingTracker.Exceptions;
using PoeRankingTracker.Models;
using System;

namespace PoeRankingTracker.Services
{
    public class LadderService : ILadderService
    {
        private static readonly Lazy<ILadderService> lazy = new Lazy<ILadderService>(() => new LadderService());
        public static ILadderService Instance { get { return lazy.Value; } }

        public const int defaultRank = 15000;

        private LadderService() {}

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
