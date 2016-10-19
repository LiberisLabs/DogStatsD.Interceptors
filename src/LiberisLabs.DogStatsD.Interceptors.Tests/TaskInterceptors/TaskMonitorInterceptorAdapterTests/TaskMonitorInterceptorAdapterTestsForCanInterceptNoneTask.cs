using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.TaskInterceptors.TaskMonitorInterceptorAdapterTests
{
    [TestFixture]
    public class TaskMonitorInterceptorAdapterTestsForCanInterceptNoneTask
    {
        private Mock<IMonitor> _monitor;
        private TaskMonitorInterceptorAdapter _adapter;

        [SetUp]
        public void GivenMonitorInterceptorAdapter()
        {
            _monitor = new Mock<IMonitor>();
            _adapter = new TaskMonitorInterceptorAdapter(_monitor.Object);
        }
        
        [Test]
        public void WhenCallingCanIntercept_ThenTheResultIsFalse()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.ReturnType)
                .Returns(typeof (int));

            var result = _adapter.CanIntercept(methodInfo.Object, null);

            Assert.That(result, Is.False);
        }
    }
}
