using System;
using System.Runtime.Serialization;

namespace PoeRankingTracker.Exceptions
{
    [Serializable]
    public class RuleNotInitializedException : Exception
    {
        public RuleNotInitializedException()
        {
        }

        public RuleNotInitializedException(string message) : base(message)
        {
        }

        public RuleNotInitializedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected RuleNotInitializedException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
    }
}
