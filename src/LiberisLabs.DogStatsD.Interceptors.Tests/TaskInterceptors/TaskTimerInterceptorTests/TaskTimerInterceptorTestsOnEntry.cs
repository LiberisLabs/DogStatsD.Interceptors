using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.TaskInterceptors.TaskTimerInterceptorTests
{
    [TestFixture]
    public class TaskTimerInterceptorTestsOnEntry
    {
        private TaskTimerInterceptor _interceptor;
        private Mock<IDogStatsD> _dogStatsD;

        [OneTimeSetUp]
        public void GivenTaskTimerInterceptor()
        {
            var statName = "namespace.classname.methodname";

            _dogStatsD = new Mock<IDogStatsD>();
            _interceptor = new TaskTimerInterceptor(_dogStatsD.Object, statName);
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