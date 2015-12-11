using LiberisLabs.DogStatsD.FunctionalTests.Helpers;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.FunctionalTests.Tests
{
    [TestFixture]
    public class InstrumentTests
    {
        [Test]
        public void GivenAServiceWithInstrumentedMethod_ThenBla()
        {
            DogStatDConfigurator.Configure();

            var instrumentationApi = new UdpListener();
            instrumentationApi.Start();

            var service = new TestServiceFactory().Create();
            service.Method();
            
            Assert.That(() => instrumentationApi.HandledPattern(@"^LiberisLabs\.DogStatsD\.FunctionalTests\.TestService\.Method\.Attempt:1|c:\\d+$"), Is.True.After(40000, 100));

            instrumentationApi.Dispose();
        }


    }
}
