using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using LiberisLabs.DogStatsD.Interceptors.Annotations;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;

namespace LiberisLabs.DogStatsD.Interceptors.TaskInterceptors
{
    public class TaskTimerInterceptor : ITaskInterceptor
    {
        private readonly IDogStatsD _dogStatsD;
        private readonly string _statName;

        private IDisposable _timer;

        public TaskTimerInterceptor(IDogStatsD dogStatsD, string statName)
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
        }

        public void OnException(Exception exception)
        {
        }

        public void OnTaskContinuation(Task task)
        {
            task.ContinueWith(x => _timer.Dispose());
        }

        public bool CanIntercept(MethodInfo methodInfo, MethodInfo methodInvocationTarget)
        {
            return typeof(Task).IsAssignableFrom(methodInfo.ReturnType)
                   && (methodInfo.HasAttribute<TimeAttribute>()
                       || methodInvocationTarget.HasAttribute<TimeAttribute>());
        }
    }
}