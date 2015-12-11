using StatsdClient;

namespace LiberisLabs.DogStatsD.FunctionalTests.Helpers
{
    public static class DogStatDConfigurator
    {
        public static void Configure()
        {
            var dogstatsdConfig = new StatsdConfig
            {
                StatsdServerName = "127.0.0.1"
            };

            DogStatsd.Configure(dogstatsdConfig);
        }
    }
}
