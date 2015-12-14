using System;
using System.Threading;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.MonitorInterceptorAdapterTests
{
    [TestFixture]
    public class MonitorInterceptorAdapterTestsForTaskReturnType
    {
        private Mock<IMonitor> _monitor;
        private MonitorInterceptorAdapter _adapter;

        [SetUp]
        public void GivenMonitorInterceptorAdapter()
        {
            _monitor = new Mock<IMonitor>();
            _adapter = new MonitorInterceptorAdapter(_monitor.Object, true);
        }

        [Test]
        public void WhenOnEntry_ThenMonitorAttemptIsCalled()
        {
            _adapter.OnEntry();

            _monitor.Verify(x => x.Attempt(), Times.Once);
        }

        [Test]
        public void WhenOnExit_ThenMonitorSuccessIsNotCalled()
        {
            _adapter.OnExit();

            _monitor.Verify(x => x.Success(), Times.Never);
        }
        
        [Test]
        public void WhenOnException_ThenMonitorErrorIsNotCalled()
        {
            _adapter.OnException(null);

            _monitor.Verify(x => x.Error(), Times.Never);
        }

        [Test]
        public async Task WhenOnTaskContinuationWithFaultedTask_ThenMonitorErrorIsCalled()
        {
            var task = Task.Run(() => { throw new Exception("Boom!"); });
            await task.ContinueWith(_adapter.OnTaskContinuation).ConfigureAwait(false);

            _monitor.Verify(x => x.Error(), Times.Once);
        }

        [Test]
        public async Task WhenOnTaskContinuationWithCanceledTask_ThenMonitorCanceledIsCalled()
        {
            var task = Task.Delay(5000, new CancellationToken(true));

            await task.ContinueWith(_adapter.OnTaskContinuation).ConfigureAwait(false);

            _monitor.Verify(x => x.Canceled(), Times.Once);
        }

        [Test]
        public async Task WhenOnTaskContinuationWithACompletedTask_ThenMonitorSuccessIsCalled()
        {
            var task = Task.FromResult(0);

            await task.ContinueWith(_adapter.OnTaskContinuation).ConfigureAwait(false);

            _monitor.Verify(x => x.Success(), Times.Once);
        }
    }
}
