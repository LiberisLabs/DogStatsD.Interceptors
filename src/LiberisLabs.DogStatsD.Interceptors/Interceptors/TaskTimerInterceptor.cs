using System;
using System.Reflection;
using System.Threading.Tasks;

namespace LiberisLabs.DogStatsD.Interceptors.Interceptors
{
    public class TaskTimerInterceptor : IInterceptor
    {
        private readonly IDogStatsD _dogStatsD;
        private readonly string _statName;

        private IDisposable _timer;

        public TaskTimerInterceptor(MethodInfo method, IDogStatsD dogStatsD)
        {
            _dogStatsD = dogStatsD;
            _statName = $"{method.ReflectedType.FullName}.{method.Name}".ToLowerInvariant();
        }

        public void OnEntry()
        {
            _timer = _dogStatsD.StartTimer(_statName);
        }

        public void OnExit()
        {
        }

        public void OnException(Exception exception)
        {
        }

        public void OnTaskContinuation(Task task)
        {
            task.ContinueWith(x => _timer.Dispose());
        }
    }
}