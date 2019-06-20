using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker.Exceptions;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PoeRankingTrackerTests.Exceptions
{
    [TestClass]
    public class CharacterNotFoundExceptionTest
    {
        private readonly string message = "Message";
        private readonly string exception = "Inner exception.";

        [TestMethod]
        public void TestSerializableException()
        {
            Exception ex = new CharacterNotFoundException(message, new Exception(exception));

            // Save the full ToString() value, including the exception message and stack trace.
            string exceptionToString = ex.ToString();

            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                // "Save" object state
                bf.Serialize(ms, ex);

                // Re-use the same stream for de-serialization
                ms.Seek(0, 0);

                // Replace the original exception with de-serialized one
                ex = (CharacterNotFoundException)bf.Deserialize(ms);
            }

            // Double-check that the exception message and stack trace (owned by the base Exception) are preserved
            Assert.AreEqual(exceptionToString, ex.ToString(), "ex.ToString()");
        }
    }
}
