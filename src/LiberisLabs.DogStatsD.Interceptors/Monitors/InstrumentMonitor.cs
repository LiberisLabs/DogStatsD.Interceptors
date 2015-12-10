using System.Reflection;

namespace LiberisLabs.DogStatsD.Interceptors.Monitors
{
    public class InstrumentMonitor : IMonitor
    {
        private readonly IDogStatsd _dogStatsd;
        private readonly string _statName;

        public InstrumentMonitor(MethodInfo method, IDogStatsd dogStatsd)
        {
            _dogStatsd = dogStatsd;
            _statName = $"{method.ReflectedType.FullName}.{method.Name}";
        }


        public void Attempt()
        {
            _dogStatsd.Increment($"{_statName}.Attempt");
        }

        public void Success()
        {
            _dogStatsd.Increment($"{_statName}.Success");
        }

        public void Error()
        {
            _dogStatsd.Increment($"{_statName}.Error");
        }

        public void Canceled()
        {
            _dogStatsd.Increment($"{_statName}.Canceled");
        }
    }
}