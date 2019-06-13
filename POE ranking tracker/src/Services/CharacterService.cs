using PoeApiClient.Models;
using PoeRankingTracker.Exceptions;
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
        long GetExperienceAhead(List<IEntry> entries, IEntry entry);
        long GetExperienceBehind(List<IEntry> entries, IEntry entry);
        IEntry GetEntry(List<IEntry> entries, string characterName);
    }

    public class CharacterService : ICharacterService
    {

        public int GetRank(ILadder ladder, string characterName)
        {
            Contract.Requires(ladder != null && characterName != null);

            foreach (var entry in ladder.Entries)
            {
                if (characterName == entry.Character.Name)
                {
                    return entry.Rank;
                }
            }

            throw new CharacterNotFoundException();
        }

        public int GetRankByClass(List<IEntry> entries, IEntry entry)
        {
            Contract.Requires(entries != null);

            var rank = 1;
            int start = entries.IndexOf(entry);
            var data = entries.ToArray();
            for (var i = start - 1; i > 0; i--)
            {
                if (entry != null && data[i].Character.CharacterClass == entry.Character.CharacterClass)
                {
                    rank++;
                }
            }
            return rank;
        }

        public int GetNumbersOfDeadsAhead(List<IEntry> entries, IEntry entry)
        {
            Contract.Requires(entries != null);

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
            Contract.Requires(entry != null);

            return entry.Dead || entry.Retired;
        }

        public long GetExperienceAhead(List<IEntry> entries, IEntry entry)
        {
            Contract.Requires(entries != null && entry != null);

            long n = 0;
            int i = entries.IndexOf(entry);
            var data = entries.ToArray();
            if (i > 0)
            {
                n = data[i - 1].Character.Experience - entry.Character.Experience;
            }
            return n;
        }

        public long GetExperienceBehind(List<IEntry> entries, IEntry entry)
        {
            Contract.Requires(entries != null && entry != null);

            long n = 0;
            int i = entries.IndexOf(entry);
            var data = entries.ToArray();
            if (i > 0 && i < data.Length - 1)
            {
                n = entry.Character.Experience - data[i + 1].Character.Experience;
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
