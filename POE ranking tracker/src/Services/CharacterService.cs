using PoeApiClient.Models;
using PoeRankingTracker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PoeRankingTracker.Services
{
    public interface ICharacterService
    {
        int GetRank(ILadder ladder, string characterName);
        int GetRankByClass(List<IEntry> entries, IEntry entry);
        int GetNumbersOfDeadsAhead(List<IEntry> entries, IEntry entry);
        bool IsEntryInvalid(IEntry entry);
        long GetExperienceDifference(IEntry entry1, IEntry entry2);
        int GetRank(List<IEntry> entries, string characterName);
        long GetExperienceAhead(List<IEntry> entries, IEntry entry);
        long GetExperienceBehind(List<IEntry> entries, IEntry entry);
        IEntry GetEntry(List<IEntry> entries, string characterName);
    }

    public class CharacterService : ICharacterService
    {
        public const int defaultRank = 15000;

        public int GetRank(ILadder ladder, string characterName)
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

        public int GetRankByClass(List<IEntry> entries, IEntry entry)
        {
            var rank = 1;
            int start = entries.IndexOf(entry);
            var data = entries.ToArray();
            for (var i = start - 1; i > 0; i--)
            {
                if (data[i].Character.CharacterClass == entry.Character.CharacterClass)
                {
                    rank++;
                }
            }
            return rank;
        }

        public int GetNumbersOfDeadsAhead(List<IEntry> entries, IEntry entry)
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

        public bool IsEntryInvalid(IEntry entry)
        {
            return entry.Dead || entry.Retired;
        }

        public long GetExperienceDifference(IEntry entry1, IEntry entry2)
        {
            return entry1.Character.Experience - entry2.Character.Experience;
        }

        public int GetRank(List<IEntry> entries, string characterName)
        {
            var rank = defaultRank;

            var match = entries.Find(e => e.Character.Name == characterName);
            if (match != null)
            {
                rank = match.Rank;
            }

            return rank;
        }

        public long GetExperienceAhead(List<IEntry> entries, IEntry entry)
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

        public long GetExperienceBehind(List<IEntry> entries, IEntry entry)
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

        public IEntry GetEntry(List<IEntry> entries, string characterName)
        {
            Contract.Requires(entries != null);

            IEntry entryFound = null;

            foreach(var entry in entries)
            {
                if (entry.Character.Name == characterName)
                {
                    entryFound = entry;
                }
            }

            return entryFound;
        }
    }
}
