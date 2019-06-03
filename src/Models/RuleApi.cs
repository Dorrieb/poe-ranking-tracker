namespace PoeRankingTracker.Models
{
    public class RuleApi
    {
        public RuleApi(int requestLimit, int interval, int timeout)
        {
            RequestLimit = requestLimit;
            Interval = interval;
            Timeout = timeout;
        }
        public int RequestLimit { get; set; }
        public int Interval { get; set; }
        public int Timeout { get; set; }
        public override string ToString()
        {
            return $"{RequestLimit}:{Interval}:{Timeout}";
        }
    }
}
