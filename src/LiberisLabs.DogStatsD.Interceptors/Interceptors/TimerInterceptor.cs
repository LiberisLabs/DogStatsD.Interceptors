using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using LiberisLabs.DogStatsD.Interceptors.Annotations;

namespace LiberisLabs.DogStatsD.Interceptors.Interceptors
{
    public class TimerInterceptor : IInterceptor
    {
        private readonly IDogStatsD _dogStatsD;
        private readonly string _statName;

        private IDisposable _timer;

        public TimerInterceptor(IDogStatsD dogStatsD, string statName)
        {
            _dogStatsD = dogStatsD;
            _statName = statName;
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

        public bool CanIntercept(MethodInfo methodInfo, MethodInfo methodInvocationTarget)
        {
            return !typeof(Task).IsAssignableFrom(methodInfo.ReturnType)
                   && (methodInfo.HasAttribute<TimeAttribute>()
                       || methodInvocationTarget.HasAttribute<TimeAttribute>());
        }
    }
}
