using PoeRankingTracker.Models;

namespace PoeRankingTracker.Services
{
    public interface ILadderService
    {
        int GetRank(Ladder ladder, string characterName);
    }
}
