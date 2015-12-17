using System;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.FunctionalTests.Helpers;
using NUnit.Framework;
using static LiberisLabs.DogStatsD.FunctionalTests.TestConstants;

namespace LiberisLabs.DogStatsD.FunctionalTests.Tests
{
    [TestFixture]
    public class InstrumentTests
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
        public void WhenCallingInstrumentMethod_ThenTheInstrumentsAreIntercepted()
        {
            _service.InstrumentMethod();
            
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumentmethod\.attempt:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumentmethod\.success:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(2).After(DelayInMilliseconds, PollingInterval));
        }

        [Test]
        public void WhenCallingInstrumentException_ThenTheInstrumentsAreIntercepted()
        {
            try
            {
                _service.InstrumentException();
            }
            catch (Exception)
            {
                // ignored
            }

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumentexception\.attempt:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumentexception\.error:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(2).After(DelayInMilliseconds, PollingInterval));
        }

        [Test]
        public async Task WhenCallingInstrumentTaskMethod_ThenTheInstrumentsAreIntercepted()
        {
            await _service.InstrumentTaskMethod().ConfigureAwait(false);

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskmethod\.attempt:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskmethod\.success:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(2).After(DelayInMilliseconds, PollingInterval));
        }

        [Test]
        public void WhenCallingInstrumentTaskException_ThenTheInstrumentsAreIntercepted()
        {
            try
            {
                _service.InstrumentTaskException();
            }
            catch (Exception)
            {
                // ignored
            }

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskexception\.attempt:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskexception\.error:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(2).After(DelayInMilliseconds, PollingInterval));
        }

        [Test]
        public async Task WhenCallingInstrumentTaskCancelled_ThenTheInstrumentsAreIntercepted()
        {
            try
            {
                await _service.InstrumentTaskCancelled().ConfigureAwait(false);
            }
            catch(Exception)
            {
                
            }
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskcancelled\.attempt:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskcancelled\.canceled:1|c:\\d+$"), Is.True.After(DelayInMilliseconds, PollingInterval));
            Assert.That(() => _instrumentationApi.StatCount(), Is.EqualTo(2).After(DelayInMilliseconds, PollingInterval));
        }

        [TearDown]
        public void Kill()
        {
            _instrumentationApi.Dispose();
        }


    }
}
