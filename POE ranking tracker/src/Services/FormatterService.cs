using PoeRankingTracker.Models;
using PoeRankingTracker.Resources.Translations;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace PoeRankingTracker.Services
{
    public interface IFormatterService
    {
        string GetFormattedNumber(int n);
        string GetFormattedExperience(long experience);
    }

    public class FormatterService: IFormatterService
    {
        public string GetFormattedNumber(int n)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:#,0}", n);
        }

        public string GetFormattedExperience(long experience)
        {
            string result;

            if (experience >= 1000000000)
            {
                result = experience.ToString("#,##0,,,B", CultureInfo.CurrentCulture);
            }
            else if (experience >= 1000000)
            {
                result = experience.ToString("#,##0,,M", CultureInfo.CurrentCulture);
            }
            else if (experience >= 1000)
            {
                result = experience.ToString("#,##0,K", CultureInfo.CurrentCulture);
            }
            else
            {
                result = experience.ToString("#,#", CultureInfo.CurrentCulture);
            }

            return result;
        }
    }
}
