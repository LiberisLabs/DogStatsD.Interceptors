using System;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.TimerInterceptorTests
{
    [TestFixture]
    public class TimerInterceptorTestsOnExit
    {
        private TimerInterceptor _interceptor;
        private Mock<IDisposable> _timer;

        [OneTimeSetUp]
        public void GivenTimerInterceptorWithInterceptorOnEntry()
        {
            _timer = new Mock<IDisposable>();
            var dogStatsD = new Mock<IDogStatsD>();
            dogStatsD.Setup(x => x.StartTimer(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string[]>()))
                .Returns(_timer.Object);

            _interceptor = new TimerInterceptor(dogStatsD.Object, "");
            _interceptor.OnEntry();
        }

        [SetUp]
        public void WhenOnExitIsCalled()
        {
            _interceptor.OnExit();
        }

        [Test]
        public void ThenTheTimerIsDisposed()
        {
            _timer.Verify(x => x.Dispose(), Times.Once);
        }
    }
}