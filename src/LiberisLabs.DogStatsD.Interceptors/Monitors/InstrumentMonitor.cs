using System.Reflection;
using Castle.Core.Internal;
using LiberisLabs.DogStatsD.Interceptors.Annotations;

namespace LiberisLabs.DogStatsD.Interceptors.Monitors
{
    public class InstrumentMonitor : IMonitor
    {
        private readonly IDogStatsD _dogStatsD;
        private readonly string _statName;

        public InstrumentMonitor(MethodInfo method, IDogStatsD dogStatsD)
        {
            _dogStatsD = dogStatsD;
            _statName = $"{method.ReflectedType.FullName}.{method.Name}".ToLowerInvariant();    
        }


        public void Attempt()
        {
            _dogStatsD.Increment($"{_statName}.attempt");
        }

        public void Success()
        {
            _dogStatsD.Increment($"{_statName}.success");
        }

        public void Error()
        {
            _dogStatsD.Increment($"{_statName}.error");
        }

        public void Canceled()
        {
            _dogStatsD.Increment($"{_statName}.canceled");
        }

        public bool CanMonitor(MethodInfo methodInfo, MethodInfo methodInvocationTarget)
        {
            return methodInfo.HasAttribute<InstrumentAttribute>()
                   || methodInvocationTarget.HasAttribute<InstrumentAttribute>();

        }
    }
}