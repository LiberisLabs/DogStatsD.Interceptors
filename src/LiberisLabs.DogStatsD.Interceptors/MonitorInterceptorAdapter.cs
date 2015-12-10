using System;
using System.Reflection;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Monitors;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class MonitorInterceptorAdapter : IInterceptor
    {
        private readonly IMonitor _monitor;
        private readonly bool _isTask;

        public MonitorInterceptorAdapter(IMonitor monitor, bool isTaskReturnType)
        {
            _monitor = monitor;
            _isTask = isTaskReturnType;
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