using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.TimerInterceptorTests
{
    [TestFixture]
    public class TimerInterceptorTestsOnEntry
    {
        private TimerInterceptor _interceptor;
        private Mock<IDogStatsD> _dogStatsD;

        [OneTimeSetUp]
        public void GivenTimerInterceptor()
        {
            var statname = "namespace.classname.methodname";

            _dogStatsD = new Mock<IDogStatsD>();
            _interceptor = new TimerInterceptor(_dogStatsD.Object, statname);
        }

        [SetUp]
        public void WhenOnEntryIsCalled()
        {
            _interceptor.OnEntry();
        }

        [Test]
        public void ThenTheTimerIsStarted()
        {
            _dogStatsD.Verify(x=> x.StartTimer("namespace.classname.methodname", 1D, null));
        }
    }
}