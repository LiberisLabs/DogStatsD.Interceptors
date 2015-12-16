using System;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using LiberisLabs.DogStatsD.Interceptors.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.TaskTimerInterceptorTests
{
    [TestFixture]
    public class TaskTimerInterceptorTestsOnExit
    {
        private TaskTimerInterceptor _interceptor;
        private Mock<IDisposable> _timer;

        [TestFixtureSetUp]
        public void GivenTaskTimerInterceptorWithInterceptorOnEntry()
        {
            var methodInfo = new MethodInfoBuilder().Create();

            _timer = new Mock<IDisposable>();
            var dogStatsD = new Mock<IDogStatsD>();
            dogStatsD.Setup(x => x.StartTimer(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string[]>()))
                .Returns(_timer.Object);

            _interceptor = new TaskTimerInterceptor(methodInfo, dogStatsD.Object);
            _interceptor.OnEntry();
        }

        [SetUp]
        public void WhenOnExitIsCalled()
        {
            _interceptor.OnTaskContinuation(Task.FromResult(0));
        }

        [Test]
        public void ThenTheTimerIsDisposed()
        {
            _timer.Verify(x => x.Dispose(), Times.Once);
        }
    }
}