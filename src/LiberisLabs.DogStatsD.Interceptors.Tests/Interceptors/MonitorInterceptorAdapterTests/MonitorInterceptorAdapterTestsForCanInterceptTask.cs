using System.Reflection;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.MonitorInterceptorAdapterTests
{
    [TestFixture]
    public class MonitorInterceptorAdapterTestsForCanInterceptTask
    {
        private Mock<IMonitor> _monitor;
        private MonitorInterceptorAdapter _adapter;

        [SetUp]
        public void GivenMonitorInterceptorAdapter()
        {
            _monitor = new Mock<IMonitor>();
            _adapter = new MonitorInterceptorAdapter(_monitor.Object);
        }
        
        [Test]
        public void WhenCallingCanIntercept_ThenTheResultIsFalse()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.ReturnType)
                .Returns(typeof (Task));

            var result = _adapter.CanIntercept(methodInfo.Object, null);

            Assert.That(result, Is.False);
        }
    }
}
