using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PoeApiClient.Services;

namespace PoeApiClientTests.Services
{
    [TestClass]
    public class SemaphoreServiceTest
    {
        private SemaphoreService semaphoreService;
        private Mock<HttpClientService> mock;

        [TestMethod]
        public void CreateSemaphore()
        {
            semaphoreService = GetService(0, 5, 10, 10);
            semaphoreService.CreateSemaphore();
            // TODO
        }

        private SemaphoreService GetService(int currentRequestLimit, int maxRequestLimit, int minInterval, int timeout)
        {
            mock = new Mock<HttpClientService>();

            mock.Setup(client => client.GetCurrentRequestLimit()).Returns(currentRequestLimit);
            mock.Setup(client => client.GetMaxRequestLimit()).Returns(maxRequestLimit);
            mock.Setup(client => client.GetMinInterval()).Returns(minInterval);
            mock.Setup(client => client.GetTimeout()).Returns(timeout);

            return new SemaphoreService(mock.Object);
        }
    }
}
