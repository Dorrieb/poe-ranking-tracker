using System;
using System.Runtime.Serialization;

namespace PoeRankingTracker.Exceptions
{
    [Serializable]
    public class CharacterNotFoundException : Exception
    {
        public CharacterNotFoundException()
        {
        }

        public CharacterNotFoundException(string message) : base(message)
        {
        }

        public CharacterNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CharacterNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
