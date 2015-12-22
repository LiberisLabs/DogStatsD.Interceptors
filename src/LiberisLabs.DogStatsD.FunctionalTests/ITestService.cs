using System.Threading.Tasks;

namespace LiberisLabs.DogStatsD.FunctionalTests
{
    public interface ITestService
    {
        void InstrumentMethod();

        void InstrumentException();

        Task InstrumentTaskMethod();

        Task InstrumentTaskException();

        Task InstrumentTaskCancelled();

        void TimeMethod();

        void TimeException();

        Task TimeTaskMethod();

        Task TimeTaskException();

        Task TimeTaskCancelled();

    }
}