using System;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Annotations;

namespace LiberisLabs.DogStatsD.FunctionalTests
{
    public class TestService : ITestService
    {
        [Instrument]
        public void InstrumentMethod()
        {
        }

        [Instrument]
        public void InstrumentException()
        {
            throw new Exception("oops!");
        }

        [Instrument]
        public async Task InstrumentTaskMethod()
        {
            await Task.FromResult(0).ConfigureAwait(false);
        }

        [Instrument]
        public async Task InstrumentTaskException()
        {
            await Task.Delay(1).ConfigureAwait(false);

            throw new Exception("oops!");
        }

        [Time]
        public void TimeMethod()
        {
            
        }

        [Time]
        public void TimeException()
        {
            throw new Exception("oops!");
        }

        [Time]
        public Task TimeTaskMethod()
        {
            return Task.FromResult(0);
        }

        [Time]
        public async Task TimeTaskException()
        {
            await Task.Delay(1).ConfigureAwait(false);

            throw new Exception("oops!");
        }
    }
}