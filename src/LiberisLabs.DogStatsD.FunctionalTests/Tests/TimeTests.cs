using System;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.FunctionalTests.Helpers;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.FunctionalTests.Tests
{
    [TestFixture]
    public class TimeTests
    {
        private ITestService _service;
        private UdpListener _instrumentationApi;

        [SetUp]
        public void GivenAConfiguredDogStatDAndAProxyService()
        {
            DogStatDConfigurator.Configure();

            _instrumentationApi = new UdpListener();
            _instrumentationApi.Start();

            _service = new TestServiceFactory().Create();
        }

        [Test]
        public void WhenCallingTimeMethod_ThenTheTimerAreIntercepted()
        {
            _service.TimeMethod();

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd.\functionaltests\.testservice\.timemethod:\\d+|ms$"), Is.True.After(40000, 100));
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

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd.\functionaltests\.testservice\.timeexception:\\d+|ms$"), Is.True.After(40000, 100));
        }


        [Test]
        public async Task WhenCallingTimeTaskMethod_ThenTheTimerAreIntercepted()
        {
            await _service.TimeTaskMethod().ConfigureAwait(false);

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd.\functionaltests\.testservice\.timetaskmethod:\\d+|ms$"), Is.True.After(40000, 100));
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

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd.\functionaltests\.testservice\.timetaskexception:\\d+|ms$"), Is.True.After(40000, 100));
        }

        [TearDown]
        public void Kill()
        {
            _instrumentationApi.Dispose();
        }
    }
}
