using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Izumi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().ConfigureLogging((c, l) =>
                    {
                        l.ClearProviders();
                        l.AddConfiguration(c.Configuration);
                        l.AddConsole();
                    });
                })
                .ConfigureAppConfiguration(x => x.AddEnvironmentVariables("Izumi_"));
    }
}
