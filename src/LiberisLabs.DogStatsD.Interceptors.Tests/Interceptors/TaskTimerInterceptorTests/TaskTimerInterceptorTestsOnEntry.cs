using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.TaskTimerInterceptorTests
{
    [TestFixture]
    public class TaskTimerInterceptorTestsOnEntry
    {
        private TaskTimerInterceptor _interceptor;
        private Mock<IDogStatsD> _dogStatsD;

        [TestFixtureSetUp]
        public void GivenTaskTimerInterceptor()
        {
            var methodInfo = new MethodInfoBuilder()
                .WithNamespace("Namespace")
                .WithClassName("ClassName")
                .WithMethodName("MethodName")
                .Create();

            _dogStatsD = new Mock<IDogStatsD>();
            _interceptor = new TaskTimerInterceptor(methodInfo, _dogStatsD.Object);
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