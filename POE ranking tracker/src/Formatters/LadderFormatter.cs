using PoeRankingTracker.Models;
using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Text;

namespace PoeRankingTracker
{
    public class LadderFormatter : JsonMediaTypeFormatter
    {
        public override object ReadFromStream(Type type, Stream readStream, Encoding effectiveEncoding, IFormatterLogger formatterLogger)
        {
            var ladder = base.ReadFromStream(type, readStream, effectiveEncoding, formatterLogger) as Ladder;

            ladder.Entries.RemoveAll(entry => entry.Dead || entry.Retired);

            return ladder;
        }
    }
}
