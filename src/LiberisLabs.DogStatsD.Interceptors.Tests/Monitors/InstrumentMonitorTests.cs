using System;
using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Monitors
{
    [TestFixture]
    public class InstrumentMonitorTests
    {
        private InstrumentMonitor _monitor;
        private Mock<IDogStatsd> _dogStatsd;
        private string _fullName;
        private string _methodName;

        [SetUp]
        public void GivenInstrumentMonitor()
        {

            var type = new Mock<Type>();
            _fullName = "Namespace.ClassName";
            type.Setup(x => x.FullName)
                .Returns(_fullName);

            var methodInfo = new Mock<MethodInfo>();
            _methodName = "MethodName";
            methodInfo.Setup(x => x.Name)
                .Returns(_methodName);
            methodInfo.Setup(x => x.ReflectedType)
                .Returns(type.Object);

            _dogStatsd = new Mock<IDogStatsd>();

            _monitor = new InstrumentMonitor(methodInfo.Object, _dogStatsd.Object);
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
    }
}
