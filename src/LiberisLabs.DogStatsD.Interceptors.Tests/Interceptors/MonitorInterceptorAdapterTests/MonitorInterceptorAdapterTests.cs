using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.MonitorInterceptorAdapterTests
{
    [TestFixture]
    public class MonitorInterceptorAdapterTests
    {
        private Mock<IMonitor> _monitor;
        private MonitorInterceptorAdapter _adapter;

        [SetUp]
        public void GivenMonitorInterceptorAdapter()
        {
            _monitor = new Mock<IMonitor>();
            _adapter = new MonitorInterceptorAdapter(_monitor.Object, false);
        }

        [Test]
        public void WhenOnEntry_ThenMonitorAttemptIsCalled()
        {
            _adapter.OnEntry();

            _monitor.Verify(x => x.Attempt(), Times.Once);
        }

        [Test]
        public void WhenOnExit_ThenMonitorSuccessIsCalled()
        {
            _adapter.OnExit();

            _monitor.Verify(x => x.Success(), Times.Once);
        }
        
        [Test]
        public void WhenOnException_ThenMonitorErrorIsCalled()
        {
            _adapter.OnException(null);

            _monitor.Verify(x => x.Error(), Times.Once);
        }
        
    }
}
