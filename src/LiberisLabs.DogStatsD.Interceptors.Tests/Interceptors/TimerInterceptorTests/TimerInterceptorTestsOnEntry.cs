using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.TimerInterceptorTests
{
    [TestFixture]
    public class TimerInterceptorTestsOnEntry
    {
        private TimerInterceptor _interceptor;
        private Mock<IDogStatsD> _dogStatsD;

        [TestFixtureSetUp]
        public void GivenTimerInterceptor()
        {
            var methodInfo = new MethodInfoBuilder()
                .WithNamespace("Namespace")
                .WithClassName("ClassName")
                .WithMethodName("MethodName")
                .Create();

            _dogStatsD = new Mock<IDogStatsD>();
            _interceptor = new TimerInterceptor(methodInfo, _dogStatsD.Object);
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