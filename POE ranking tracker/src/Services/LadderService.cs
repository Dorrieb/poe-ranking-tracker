using PoeRankingTracker.Models;
using System;

namespace PoeRankingTracker.Services
{
    public interface ILadderService
    {
        int GetRank(Ladder ladder, string characterName);
    }

    public class LadderService : ILadderService
    {
        public const int defaultRank = 15000;

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
