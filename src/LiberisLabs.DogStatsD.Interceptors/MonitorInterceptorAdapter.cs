using System;
using System.Reflection;
using System.Threading.Tasks;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class MonitorInterceptorAdapter : IInterceptor
    {
        private readonly IMonitor _monitor;
        private readonly bool _isTask;

        public MonitorInterceptorAdapter(IMonitor monitor, MethodInfo method)
        {
            _monitor = monitor;
            _isTask = typeof(Task).IsAssignableFrom(method.ReturnType);
        }

        public void OnEntry()
        {
            _monitor.Attempt();
        }

        public void OnExit()
        {
            if (!_isTask)
            {
                _monitor.Success();
            }
        }

        public void OnException(Exception exception)
        {
            if (!_isTask)
            {
                _monitor.Error();
            }
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
    }
}