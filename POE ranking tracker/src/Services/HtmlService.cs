using PoeApiClient.Models;
using PoeRankingTracker.Models;
using PoeRankingTracker.Resources.Translations;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

namespace PoeRankingTracker.Services
{
    public interface IHtmlService
    {
        HtmlConfiguration BuildHtmlConfiguration(List<IEntry> entries, IEntry entry);
        string UpdateContent(string content, HtmlConfiguration configuration);
        string GetTemplate(string templatePath);
    }

    public class HtmlService: IHtmlService
    {
        private readonly IFormatterService formatterService;
        private readonly ICharacterService characterService;

        public HtmlService(IFormatterService formatterService, ICharacterService characterService)
        {
            this.formatterService = formatterService;
            this.characterService = characterService;
        }

        public HtmlConfiguration BuildHtmlConfiguration(List<IEntry> entries, IEntry entry)
        {
            Contract.Requires(entry != null);

            var configuration = new HtmlConfiguration()
            {
                CharacterClass = entry.Character.CharacterClass,
                Level = entry.Character.Level,
                Rank = entry.Rank,
            };

            if (entries != null)
            {
                var rankByClass = 1;
                var deadsAhead = 0;
                int start = entries.IndexOf(entry);
                var data = entries.ToArray();
                for (var i = start - 1; i >= 0; i--)
                {
                    if (data[i].Character.CharacterClass == entry.Character.CharacterClass)
                    {
                        rankByClass++;
                    }

                    if (characterService.IsEntryInvalid(data[i]))
                    {
                        deadsAhead++;
                    }
                }
                configuration.RankByClass = rankByClass;
                configuration.DeadsAhead = deadsAhead;
                configuration.ExperienceAhead = characterService.GetExperienceAhead(entries, entry);
                configuration.ExperienceBehind = characterService.GetExperienceBehind(entries, entry);
            }

            return configuration;
        }

        public string UpdateContent(string content, HtmlConfiguration configuration)
        {
            Contract.Requires(content != null && configuration != null);

            content = content.Replace("{class}", configuration.CharacterClass.ToString());
            content = content.Replace("{level}", $"{configuration.Level}");
            content = content.Replace("{rankLabel}", Strings.GlobalRank);
            content = content.Replace("{rank}", formatterService.GetFormattedNumber(configuration.Rank));
            content = content.Replace("{rankByClassLabel}", Strings.ClassRank);
            var rankByClass = formatterService.GetFormattedNumber(configuration.RankByClass);
            if (rankByClass == "0")
            {
                rankByClass = "-";
            }
            content = content.Replace("{rankByClass}", rankByClass);
            content = content.Replace("{experienceAheadLabel}", formatterService.GetFormattedNumber(configuration.Rank - 1));
            var experienceAhead = formatterService.GetFormattedExperience(configuration.ExperienceAhead);
            if (experienceAhead.Length == 0)
            {
                experienceAhead = "-";
            };
            content = content.Replace("{experienceAhead}", experienceAhead);
            content = content.Replace("{experienceBehindLabel}", formatterService.GetFormattedNumber(configuration.Rank + 1));
            var experienceBehind = formatterService.GetFormattedExperience(configuration.ExperienceBehind);
            if (experienceBehind.Length == 0)
            {
                experienceBehind = "-";
            };
            content = content.Replace("{experienceBehind}", experienceBehind);
            content = content.Replace("{deadsAheadLabel}", Strings.DeadsAhead);
            var deadsAhead = formatterService.GetFormattedNumber(configuration.DeadsAhead);
            if (deadsAhead == "0")
            {
                deadsAhead = "-";
            }
            content = content.Replace("{deadsAhead}", deadsAhead);

            return content;
        }

        public string GetTemplate(string templatePath)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            var filePath = $"{currentDirectory}/{templatePath}";
            return File.ReadAllText(filePath);
        }
    }
}
