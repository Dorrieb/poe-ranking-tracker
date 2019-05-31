using PoeRankingTracker.Models;
using System.Collections.Generic;

namespace PoeRankingTracker.Services
{
    public interface ICharacterService
    {
        int GetRankByClass(List<Entry> entries, Entry entry);
        int GetNumbersOfDeadsAhead(List<Entry> entries, Entry entry);
        bool IsEntryInvalid(Entry entry);
        long GetExperienceDifference(Entry entry1, Entry entry2);
        int GetRank(List<Entry> entries, string characterName);
        long GetExperienceAhead(List<Entry> entries, Entry entry);
        long GetExperienceBehind(List<Entry> entries, Entry entry);
    }
}
