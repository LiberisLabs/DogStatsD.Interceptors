using System;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.FunctionalTests.Helpers;
using NUnit.Framework;
using static LiberisLabs.DogStatsD.FunctionalTests.TestConstants;

namespace LiberisLabs.DogStatsD.FunctionalTests.Tests
{
    [TestFixture]
    public class TimeTests
    {
        private ITestService _service;
        private UdpListener _instrumentationApi;

        [SetUp]
        public void GivenAConfiguredDogStatsDAndAProxyService()
        {
            DogStatsDConfigurator.Configure();

            _instrumentationApi = new UdpListener();
            _instrumentationApi.Start();

            _service = new TestServiceFactory().Create();
        }

        [Test]
        public void WhenCallingTimeMethod_ThenTheTimerAreIntercepted()
        {
            _service.TimeMethod();

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd.\functionaltests\.testservice\.timemethod:\\d+|ms$"), Is.True.After(DelayInMilliseconds, 100));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(1).After(DelayInMilliseconds, 100));
        }

        [Test]
        public void WhenCallingTimeException_ThenTheTimerAreIntercepted()
        {
            try
            {
                _service.TimeException();
            }
            catch (Exception)
            {
                // ignored
            }

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd.\functionaltests\.testservice\.timeexception:\\d+|ms$"), Is.True.After(DelayInMilliseconds, 100));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(1).After(DelayInMilliseconds, 100));
        }


        [Test]
        public async Task WhenCallingTimeTaskMethod_ThenTheTimerAreIntercepted()
        {
            await _service.TimeTaskMethod().ConfigureAwait(false);

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd.\functionaltests\.testservice\.timetaskmethod:\\d+|ms$"), Is.True.After(DelayInMilliseconds, 100));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(1).After(DelayInMilliseconds, 100));
        }

        [Test]
        public async Task WhenCallingTimeTaskException_ThenTheTimerAreIntercepted()
        {
            try
            {
                await _service.TimeTaskException().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // ignored
            }

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd.\functionaltests\.testservice\.timetaskexception:\\d+|ms$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(1).After(DelayInMilliseconds, PollingInterval));
        }

        [Test]
        public async Task WhenCallingTimeTaskCancelled_ThenTheTimerAreIntercepted()
        {
            try
            {
                await _service.TimeTaskCancelled().ConfigureAwait(false);
            }
            catch (Exception)
            {

            }
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd.\functionaltests\.testservice\.timetaskcancelled:\\d+|ms$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(1).After(DelayInMilliseconds, PollingInterval));
        }

        [TearDown]
        public void Kill()
        {
            _instrumentationApi.Dispose();
        }
    }
}
