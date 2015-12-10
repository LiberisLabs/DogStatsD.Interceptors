using System;
using StatsdClient;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class DogStatsdWrapper : IDogStatsd
    {
        public void Counter<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            DogStatsd.Counter(statName, value, sampleRate, tags);
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, params string[] tags)
        {
            DogStatsd.Decrement(statName, value, sampleRate, tags);

        }

        public void Event(string title, string text, string alertType = null, string aggregationKey = null, string sourceType = null,
            int? dateHappened = null, string priority = null, string hostname = null, string[] tags = null)
        {
            DogStatsd.Event(title, text, alertType, aggregationKey, sourceType, dateHappened, priority, hostname, tags);
        }

        public void Gauge<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            DogStatsd.Gauge(statName, value, sampleRate, tags);
        }

        public void Histogram<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            DogStatsd.Histogram(statName, value, sampleRate, tags);
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
            DogStatsd.Increment(statName, value, sampleRate, tags);
        }

        public void Set<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            DogStatsd.Set(statName, value, sampleRate, tags);
        }

        public IDisposable StartTimer(string name, double sampleRate = 1, string[] tags = null)
        {
            return DogStatsd.StartTimer(name, sampleRate, tags);
        }

        public void Time(Action action, string statName, double sampleRate = 1, string[] tags = null)
        {
            DogStatsd.Time(action, statName, sampleRate, tags);
        }

        public T Time<T>(Func<T> func, string statName, double sampleRate = 1, string[] tags = null)
        {
            return DogStatsd.Time(func, statName, sampleRate, tags);
        }

        public void Timer<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            DogStatsd.Timer<T>(statName, value, sampleRate, tags);
        }
    }
}