namespace LiberisLabs.DogStatsD.Interceptors.Monitors
{
    public interface IMonitor
    {
        void Attempt();

        void Success();

        void Error();

        void Canceled();
    }
}