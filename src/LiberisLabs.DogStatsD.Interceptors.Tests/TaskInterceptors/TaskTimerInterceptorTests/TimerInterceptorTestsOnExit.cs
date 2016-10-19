using System;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.TaskInterceptors.TaskTimerInterceptorTests
{
    [TestFixture]
    public class TaskTimerInterceptorTestsOnExit
    {
        private TaskTimerInterceptor _interceptor;
        private Mock<IDisposable> _timer;

        [OneTimeSetUp]
        public void GivenTaskTimerInterceptorWithInterceptorOnEntry()
        {
            var statName = "namespace.classname.methodname";

            _timer = new Mock<IDisposable>();
            var dogStatsD = new Mock<IDogStatsD>();
            dogStatsD.Setup(x => x.StartTimer(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string[]>()))
                .Returns(_timer.Object);

            _interceptor = new TaskTimerInterceptor(dogStatsD.Object, statName);
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