using System;
using System.Threading.Tasks;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public interface IMonitor
    {
        void Attempt();

        void Success();

        void Error();

        void Canceled();
    }
}