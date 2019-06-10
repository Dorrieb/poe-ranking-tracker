namespace PoeApiClient.Models
{
    public interface IRuleApi
    {
        int RequestLimit { get; set; }
        int Interval { get; set; }
        int Timeout { get; set; }
    }

    public class RuleApi : IRuleApi
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
