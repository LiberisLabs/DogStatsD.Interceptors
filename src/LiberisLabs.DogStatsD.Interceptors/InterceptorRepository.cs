using System.Collections.Generic;
using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Monitors;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class InterceptorRepository : IInterceptorRepository
    {
        private readonly IDogStatsD _dogStatsD;

        public InterceptorRepository()
        {
            _dogStatsD = new DogStatsDWrapper();
        }

        public IList<ITaskInterceptor> GetAll(string statName)
        {
            var interceptors = new List<ITaskInterceptor>();
            interceptors.AddRange(WrapMonitorWithAdapter(new InstrumentMonitor(_dogStatsD, statName)));
            interceptors.Add(new TaskTimerInterceptor(_dogStatsD, statName));
            interceptors.Add(WrapInterceptorWithAdapter(new TimerInterceptor(_dogStatsD, statName)));

            return interceptors;
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