using System.Threading.Tasks;

namespace LiberisLabs.DogStatsD.FunctionalTests
{
    public interface ITestService
    {
        void InstrumentMethod();

        void InstrumentException();

        Task InstrumentTaskMethod();

        Task InstrumentTaskException();

        void TimeMethod();

        void TimeException();
    }
}