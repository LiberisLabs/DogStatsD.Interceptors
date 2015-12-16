using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class InterceptorFactory : IInterceptorFactory
    {
        private readonly IDogStatsD _dogStatsD;

        public InterceptorFactory()
        {
            _dogStatsD = new DogStatsDWrapper();
        }

        public ICollection<ITaskInterceptor> CreateInterceptors(MethodInfo method, MethodInfo methodInvocationTarget)
        {
            var applicableInterceptors = CreateAllInterceptors(method, methodInvocationTarget)
                .Where(x => x.CanIntercept(method, methodInvocationTarget))
                .ToList();

            return applicableInterceptors;
        }

        private List<ITaskInterceptor> CreateAllInterceptors(MethodInfo method, MethodInfo methodInvocationTarget)
        {
            var statName = CreateStatName(methodInvocationTarget);
            var interceptors = new List<ITaskInterceptor>();
            interceptors.AddRange(WrapMonitorWithAdapter(new InstrumentMonitor(_dogStatsD, statName)));
            interceptors.Add(new TaskTimerInterceptor(_dogStatsD, statName));
            interceptors.Add(WrapInterceptorWithAdapter(new TimerInterceptor(_dogStatsD, statName)));

            return interceptors;
        }

        private string CreateStatName(MethodInfo methodInvocationTarget)
        {
            return $"{methodInvocationTarget.ReflectedType.FullName}.{methodInvocationTarget.Name}".ToLowerInvariant();
        }

        private static ITaskInterceptor[] WrapMonitorWithAdapter(IMonitor monitor)
        {
            return new[]
            {
                WrapInterceptorWithAdapter(new MonitorInterceptorAdapter(monitor)),
                new TaskMonitorInterceptorAdapter(monitor)
            };
        }

        private static ITaskInterceptor WrapInterceptorWithAdapter(IInterceptor interceptor)
        {
            return new InterceptorAdapter(interceptor);
        }       
    }
}