using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SimpleFunctionApp
{
    public static class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(c =>
                {
                    c.AddEnvironmentVariables();
                })
                .ConfigureFunctionsWorkerDefaults()
                .Build();

            host.Run();
        }
    }
}