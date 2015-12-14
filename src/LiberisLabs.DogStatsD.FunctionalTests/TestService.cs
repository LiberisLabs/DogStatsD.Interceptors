using System;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Annotations;

namespace LiberisLabs.DogStatsD.FunctionalTests
{
    public class TestService : ITestService
    {
        [Instrument]
        public void Method()
        {
            Console.WriteLine("Method1");
        }

        [Instrument]
        public void Exception()
        {
            throw new Exception("oops!");
        }

        [Instrument]
        public async Task TaskMethod()
        {
            await Task.FromResult(0).ConfigureAwait(false);
        }

        [Instrument]
        public async Task TaskException()
        {
            await Task.Delay(1).ConfigureAwait(false);

            throw new Exception("oops!");
        }
    }
}