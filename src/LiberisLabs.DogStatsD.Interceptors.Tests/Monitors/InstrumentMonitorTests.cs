﻿using LiberisLabs.DogStatsD.Interceptors.Monitors;
using LiberisLabs.DogStatsD.Interceptors.Tests.Helpers;
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
    }
}
