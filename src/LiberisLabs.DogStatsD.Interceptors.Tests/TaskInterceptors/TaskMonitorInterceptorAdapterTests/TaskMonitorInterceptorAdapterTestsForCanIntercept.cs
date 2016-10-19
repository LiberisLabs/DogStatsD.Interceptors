using System.Reflection;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.TaskInterceptors.TaskMonitorInterceptorAdapterTests
{
    [TestFixture]
    public class TaskMonitorInterceptorAdapterTestsForCanIntercept
    {
        private Mock<IMonitor> _monitor;
        private TaskMonitorInterceptorAdapter _adapter;

        [SetUp]
        public void GivenMonitorInterceptorAdapter()
        {
            _monitor = new Mock<IMonitor>();
            _monitor.Setup(x => x.CanMonitor(It.IsAny<MethodInfo>(), It.IsAny<MethodInfo>()))
                .Returns(true);

            _adapter = new TaskMonitorInterceptorAdapter(_monitor.Object);
        }

        [Test]
        public void WhenCallingCanIntercept_ThenTheResultIsFalse()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.ReturnType)
                .Returns(typeof(Task));

            var result = _adapter.CanIntercept(methodInfo.Object, null);

            Assert.That(result, Is.True);
        }
    }
}