using PoeRankingTracker.Models;
using System;
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

    public class CharacterService : ICharacterService
    {
        public int GetRankByClass(List<Entry> entries, Entry entry)
        {
            var rank = 1;
            int start = entries.IndexOf(entry);
            var data = entries.ToArray();
            for (var i = start - 1; i > 0; i--)
            {
                if (data[i].Character.Class == entry.Character.Class)
                {
                    rank++;
                }
            }
            return rank;
        }

        public int GetNumbersOfDeadsAhead(List<Entry> entries, Entry entry)
        {
            var n = 0;
            int start = entries.IndexOf(entry);
            var data = entries.ToArray();
            for (var i = start - 1; i >= 0 ; i--)
            {
                if (IsEntryInvalid(data[i]))
                {
                    n++;
                }
            }
            return n;
        }

        public bool IsEntryInvalid(Entry entry)
        {
            return entry.Dead || entry.Retired;
        }

        public long GetExperienceDifference(Entry entry1, Entry entry2)
        {
            return entry1.Character.Experience - entry2.Character.Experience;
        }

        public int GetRank(List<Entry> entries, string characterName)
        {
            var rank = LadderService.defaultRank;

            var match = entries.Find(e => e.Character.Name == characterName);
            if (match != null)
            {
                rank = match.Rank;
            }

            return rank;
        }

        public long GetExperienceAhead(List<Entry> entries, Entry entry)
        {
            long n = 0;
            int i = entries.IndexOf(entry);
            var data = entries.ToArray();
            if (i > 0)
            {
                n = GetExperienceDifference(data[i - 1], entry);
            }
            return n;
        }

        public long GetExperienceBehind(List<Entry> entries, Entry entry)
        {
            long n = 0;
            int i = entries.IndexOf(entry);
            var data = entries.ToArray();
            if (i > 0 && i < data.Length - 1)
            {
                n = GetExperienceDifference(entry, data[i + 1]);
            }
            return n;
        }
    }
}
