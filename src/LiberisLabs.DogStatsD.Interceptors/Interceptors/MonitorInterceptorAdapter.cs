using System;
using System.Reflection;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Monitors;

namespace LiberisLabs.DogStatsD.Interceptors.Interceptors
{
    public class MonitorInterceptorAdapter : IInterceptor
    {
        private readonly IMonitor _monitor;

        public MonitorInterceptorAdapter(IMonitor monitor)
        {
            _monitor = monitor;
        }

        public void OnEntry()
        {
            _monitor.Attempt();
        }

        public void OnExit()
        {
            _monitor.Success();
        }

        public void OnException(Exception exception)
        {
            _monitor.Error();
        }

        public bool CanIntercept(MethodInfo methodInfo, MethodInfo methodInvocationTarget)
        {
            return !typeof(Task).IsAssignableFrom(methodInfo.ReturnType) && _monitor.CanMonitor(methodInfo, methodInvocationTarget);
        }
    }
}