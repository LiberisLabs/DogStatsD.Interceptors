using System;
using System.Reflection;
using System.Threading.Tasks;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class DataDogInstrumentMonitor : IMonitor
    {
        private readonly IDogStatsd _dogStatsd;
        private readonly bool _isTask;
        private readonly string _statName;

        public DataDogInstrumentMonitor(MethodInfo method, IDogStatsd dogStatsd)
        {
            _dogStatsd = dogStatsd;
            _statName = $"{method.ReflectedType.FullName}.{method.Name}";
            _isTask = typeof (Task).IsAssignableFrom(method.ReturnType);
        }

        public void OnEntry()
        {
            Attempt();
        }

        public void OnExit()
        {
            if (!_isTask)
            {
                Success();
            }
        }
        
        public void OnException(Exception exception)
        {
            if (!_isTask)
            {
                Error();
            }
        }

        public void OnTaskContinuation(Task task)
        {
            if (task.IsCanceled)
            {
                Canceled();
            }
            else if (task.IsFaulted)
            {
                Error();
            }
            else if (task.IsCompleted)
            {
                Success();
            }
        }

        private void Attempt()
        {
            _dogStatsd.Increment($"{_statName}.Attempt");
        }

        private void Success()
        {
            _dogStatsd.Increment($"{_statName}.Success");
        }

        private void Error()
        {
            _dogStatsd.Increment($"{_statName}.Error");
        }

        private void Canceled()
        {
            Console.WriteLine($"{_statName}.Canceled");
        }
    }
}