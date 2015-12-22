using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using LiberisLabs.DogStatsD.Interceptors.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.TaskInterceptors.TaskTimerInterceptorTests
{
    [TestFixture]
    public class TaskTimerInterceptorTestsOnEntry
    {
        private TaskTimerInterceptor _interceptor;
        private Mock<IDogStatsD> _dogStatsD;

        [TestFixtureSetUp]
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