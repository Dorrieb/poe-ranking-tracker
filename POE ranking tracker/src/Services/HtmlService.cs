using HtmlAgilityPack;
using PoeApiClient.Models;
using PoeRankingTracker.Models;
using PoeRankingTracker.Resources.Translations;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

namespace PoeRankingTracker.Services
{
    public interface IHtmlService
    {
        void SetContent(string content);
        HtmlConfiguration BuildHtmlConfiguration(List<IEntry> entries, IEntry entry);
        string UpdateContent(HtmlConfiguration configuration, bool setProgressToMax);
        string GetTemplate(string templatePath);
    }

    public class HtmlService: IHtmlService
    {
        private readonly IFormatterService formatterService;
        private readonly ICharacterService characterService;
        private readonly HtmlDocument document;

        public HtmlService(IFormatterService formatterService, ICharacterService characterService)
        {
            this.formatterService = formatterService;
            this.characterService = characterService;
            this.document = new HtmlDocument();
        }

        public void SetContent(string content)
        {
            Contract.Requires(content != null);

            document.LoadHtml(content);
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

        public string UpdateContent(HtmlConfiguration configuration, bool setProgressToMax)
        {
            Contract.Requires(configuration != null);

            SetNodeHtml(document, "class", configuration.CharacterClass.ToString());
            SetNodeHtml(document, "level", $"{configuration.Level}");
            SetNodeHtml(document, "rank-label", Strings.GlobalRank);
            SetNodeHtml(document, "rank-value", formatterService.GetFormattedNumber(configuration.Rank));
            SetNodeHtml(document, "rank-by-class-label", Strings.ClassRank);
            var rankByClass = formatterService.GetFormattedNumber(configuration.RankByClass);
            if (rankByClass == "0")
            {
                rankByClass = "-";
            }
            SetNodeHtml(document, "rank-by-class-value", rankByClass);
            SetNodeHtml(document, "experience-ahead-label", formatterService.GetFormattedNumber(configuration.Rank - 1));
            var experienceAhead = formatterService.GetFormattedExperience(configuration.ExperienceAhead);
            if (experienceAhead.Length == 0)
            {
                experienceAhead = "-";
            };
            SetNodeHtml(document, "experience-ahead-value", experienceAhead);
            SetNodeHtml(document, "experience-behind-label", formatterService.GetFormattedNumber(configuration.Rank + 1));
            var experienceBehind = formatterService.GetFormattedExperience(configuration.ExperienceBehind);
            if (experienceBehind.Length == 0)
            {
                experienceBehind = "-";
            };
            SetNodeHtml(document, "experience-behind-value", experienceBehind);
            SetNodeHtml(document, "deads-ahead-label", Strings.DeadsAhead);
            var deadsAhead = formatterService.GetFormattedNumber(configuration.DeadsAhead);
            if (deadsAhead == "0")
            {
                deadsAhead = "-";
            }
            SetNodeHtml(document, "deads-ahead-value", deadsAhead);

            if (setProgressToMax)
            {
                SetProgressBarToMaxValue(document);
            }

            return document.DocumentNode.InnerHtml;
        }

        public string GetTemplate(string templatePath)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            var filePath = $"{currentDirectory}/{templatePath}";
            return File.ReadAllText(filePath);
        }

        private void SetNodeHtml(HtmlDocument document, string nodeId, string value)
        {
            var node = document.GetElementbyId(nodeId);
            if (node != null)
            {
                node.InnerHtml = value;
            }
        }

        private void SetProgressBarToMaxValue(HtmlDocument document)
        {
            var progress = document.GetElementbyId("progress");
            if (progress != null)
            {
                progress.SetAttributeValue("value", "100");
                progress.SetAttributeValue("max", "100");
            }
        }
    }
}
