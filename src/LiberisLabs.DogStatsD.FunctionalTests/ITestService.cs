using System.Threading.Tasks;

namespace LiberisLabs.DogStatsD.FunctionalTests
{
    public interface ITestService
    {
        void Method();

        void Exception();

        Task TaskMethod();

        Task TaskException();
    }
}