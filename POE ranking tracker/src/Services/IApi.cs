using PoeRankingTracker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoeRankingTracker.Services
{
    public interface IApi
    {
        Task SetSessionId(string sessionId);
        Task<List<League>> GetLeaguesAsync();
        Task<League> GetLeagueAsync(string leagueId);
        Task<Ladder> GetLadderAsync(string leagueId, string accountName);
        Task<Ladder> GetLadderAsync(string leagueId, int offset, int limit);
        Task<List<Entry>> GetEntries(string leagueId, string accountName, string characterName);
        void CancelTasks();
        bool SessionIdCorrect();
        event EventHandler<ApiEventArgs> GetEntriesStarted;
        event EventHandler<ApiEventArgs> GetEntriesIncremented;
        event EventHandler<ApiEventArgs> GetEntriesEnded;
    }
}
