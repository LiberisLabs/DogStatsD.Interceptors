﻿using System;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.FunctionalTests.Helpers;
using NUnit.Framework;

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
            
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumentmethod\.attempt:1|c:\\d+$"), Is.True.After(40000, 100));
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumentmethod\.success:1|c:\\d+$"), Is.True.After(40000, 100));
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

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumentexception\.attempt:1|c:\\d+$"), Is.True.After(40000, 100));
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumentexception\.error:1|c:\\d+$"), Is.True.After(40000, 100));
        }

        [Test]
        public async Task WhenCallingInstrumentTaskMethod_ThenTheInstrumentsAreIntercepted()
        {
            await _service.InstrumentTaskMethod().ConfigureAwait(false);

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskmethod\.attempt:1|c:\\d+$"), Is.True.After(40000, 100));
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskmethod\.success:1|c:\\d+$"), Is.True.After(40000, 100));
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

            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskexception\.attempt:1|c:\\d+$"), Is.True.After(40000, 100));
            Assert.That(() => _instrumentationApi.HandledPattern(@"^liberislabs\.dogstatsd\.functionaltests\.testservice\.instrumenttaskexception\.error:1|c:\\d+$"), Is.True.After(40000, 100));
        }

        [TearDown]
        public void Kill()
        {
            _instrumentationApi.Dispose();
        }


    }
}