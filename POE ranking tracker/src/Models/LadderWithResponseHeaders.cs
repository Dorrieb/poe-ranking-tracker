using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PoeRankingTracker.Models
{
    public class LadderWithResponseHeaders
    {
        public LadderWithResponseHeaders(Ladder ladder, RuleApi rule, RuleApi ruleState)
        {
            Ladder = ladder;
            Rule = rule;
            RuleState = ruleState;
        }

        public Ladder Ladder { get; set; }
        public RuleApi Rule { get; set; }
        public RuleApi RuleState { get; set; }
    }
}
