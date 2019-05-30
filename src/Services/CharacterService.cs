using PoeRankingTracker.Models;
using System;
using System.Collections.Generic;

namespace PoeRankingTracker.Services
{
    public class CharacterService : ICharacterService
    {
        private static readonly Lazy<ICharacterService> lazy = new Lazy<ICharacterService>(() => new CharacterService());
        public static ICharacterService Instance { get { return lazy.Value; } }

        private CharacterService() {}

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
            if (i > 0)
            {
                n = GetExperienceDifference(entry, data[i + 1]);
            }
            return n;
        }
    }
}
