using System;
using System.Reflection;
using System.Threading.Tasks;

namespace LiberisLabs.DogStatsD.Interceptors.Interceptors
{
    public class TimerInterceptor : IInterceptor
    {
        private readonly IDogStatsD _dogStatsD;
        private readonly string _statName;

        private IDisposable _timer;

        public TimerInterceptor(MethodInfo method, IDogStatsD dogStatsD)
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
            _timer.Dispose();
        }

        public void OnException(Exception exception)
        {
            _timer.Dispose();
        }

        public void OnTaskContinuation(Task task)
        {
            throw new NotImplementedException();
        }
    }
}
