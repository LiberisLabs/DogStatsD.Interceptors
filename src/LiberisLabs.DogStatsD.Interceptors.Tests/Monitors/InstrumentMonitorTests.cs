using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.Annotations;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Monitors
{
    [TestFixture]
    public class InstrumentMonitorTests
    {
        private InstrumentMonitor _monitor;
        private Mock<IDogStatsD> _dogStatsd;

        [SetUp]
        public void GivenInstrumentMonitor()
        {
            var statname = "namespace.classname.methodname";

            _dogStatsd = new Mock<IDogStatsD>();

            _monitor = new InstrumentMonitor(_dogStatsd.Object, statname);
        }

        [Test]
        public void WhenMonitoringAnAttempt_ThenDogStatsDIsIncremented()
        {
            _monitor.Attempt();

            _dogStatsd.Verify(x => x.Increment("namespace.classname.methodname.attempt", 1, 1D, null), Times.Once);
        }

        [Test]
        public void WhenMonitoringAnCanceled_ThenDogStatsDIsIncremented()
        {
            _monitor.Canceled();

            _dogStatsd.Verify(x => x.Increment("namespace.classname.methodname.canceled", 1, 1D, null), Times.Once);
        }

        [Test]
        public void WhenMonitoringAnError_ThenDogStatsDIsIncremented()
        {
            _monitor.Error();

            _dogStatsd.Verify(x => x.Increment("namespace.classname.methodname.error", 1, 1D, null), Times.Once);
        }

        [Test]
        public void WhenMonitoringAnSuccess_ThenDogStatsDIsIncremented()
        {
            _monitor.Success();

            _dogStatsd.Verify(x => x.Increment("namespace.classname.methodname.success", 1, 1D, null), Times.Once);
        }

        [Test]
        public void WhenCallingCanMonitorWithNoAttibutesOn_ReturnsFalse()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.GetCustomAttributes(typeof(InstrumentAttribute), false))
                .Returns(new InstrumentAttribute[0]);

            var result = _monitor.CanMonitor(methodInfo.Object, methodInfo.Object);

            Assert.That(result, Is.False);
        }
        
        [Test]
        public void WhenCallingCanMonitorWithAttibutesOnMethodInfo_ReturnsTrue()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.GetCustomAttributes(typeof (InstrumentAttribute), false))
                .Returns(new[] {new InstrumentAttribute()});

            var methodInvocationTarget = new Mock<MethodInfo>();
            methodInvocationTarget.Setup(x => x.GetCustomAttributes(typeof(InstrumentAttribute), false))
                .Returns(new InstrumentAttribute[0]);

            var result = _monitor.CanMonitor(methodInfo.Object, methodInvocationTarget.Object);

            Assert.That(result, Is.True);
        }

        [Test]
        public void WhenCallingCanMonitorWithAttibutesOnMethodInvocationTarget_ReturnsTrue()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.GetCustomAttributes(typeof(InstrumentAttribute), false))
                .Returns(new InstrumentAttribute[0]);

            var methodInvocationTarget = new Mock<MethodInfo>();
            methodInvocationTarget.Setup(x => x.GetCustomAttributes(typeof(InstrumentAttribute), false))
                .Returns(new[] { new InstrumentAttribute() });

            var result = _monitor.CanMonitor(methodInfo.Object, methodInvocationTarget.Object);

            Assert.That(result, Is.True);
        }

        [Test]
        public void WhenCallingCanMonitorWithAttibutesBothMethodInfo_ReturnsTrue()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.GetCustomAttributes(typeof(InstrumentAttribute), false))
                .Returns(new[] { new InstrumentAttribute() });

            var result = _monitor.CanMonitor(methodInfo.Object, methodInfo.Object);

            Assert.That(result, Is.True);
        }
    }
}
