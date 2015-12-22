using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.MonitorInterceptorAdapterTests
{
    [TestFixture]
    public class MonitorInterceptorAdapterTestsForCanIntercept
    {
        private Mock<IMonitor> _monitor;
        private MonitorInterceptorAdapter _adapter;

        [SetUp]
        public void GivenMonitorInterceptorAdapter()
        {
            _monitor = new Mock<IMonitor>();
            _monitor.Setup(x => x.CanMonitor(It.IsAny<MethodInfo>(), It.IsAny<MethodInfo>()))
                .Returns(true);

            _adapter = new MonitorInterceptorAdapter(_monitor.Object);
        }

        [Test]
        public void WhenCallingCanIntercept_ThenTheResultIsFalse()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.ReturnType)
                .Returns(typeof(int));

            var result = _adapter.CanIntercept(methodInfo.Object, null);

            Assert.That(result, Is.True);
        }
    }
}