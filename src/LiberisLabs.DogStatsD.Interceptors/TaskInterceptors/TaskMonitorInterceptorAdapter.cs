using System;
using System.Reflection;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Monitors;

namespace LiberisLabs.DogStatsD.Interceptors.TaskInterceptors
{
    public class TaskMonitorInterceptorAdapter : ITaskInterceptor
    {
        private readonly IMonitor _monitor;

        public TaskMonitorInterceptorAdapter(IMonitor monitor)
        {
            _monitor = monitor;
        }

        public void OnEntry()
        {
            _monitor.Attempt();
        }

        public void OnExit()
        {
        }

        public void OnException(Exception exception)
        {
        }

        public void OnTaskContinuation(Task task)
        {
            if (task.IsCanceled)
            {
                _monitor.Canceled();
            }
            else if (task.IsFaulted)
            {
                _monitor.Error();
            }
            else if (task.IsCompleted)
            {
                _monitor.Success();
            }
        }

        public bool CanIntercept(MethodInfo methodInfo, MethodInfo methodInvocationTarget)
        {
            return typeof(Task).IsAssignableFrom(methodInfo.ReturnType) && _monitor.CanMonitor(methodInfo, methodInvocationTarget);
        }
    }
}