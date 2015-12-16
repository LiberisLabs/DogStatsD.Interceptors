using System.Reflection;

namespace LiberisLabs.DogStatsD.Interceptors.Monitors
{
    public interface IMonitor
    {
        void Attempt();

        void Success();

        void Error();

        void Canceled();
        bool CanMonitor(MethodInfo methodInfo, MethodInfo methodInvocationTarget);
    }
}