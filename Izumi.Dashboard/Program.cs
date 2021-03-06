using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace Izumi.Dashboard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(_ => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.IsDevelopment()
                    ? "https://localhost:5001/"
                    : "https://izumi.envyvox.app/")
            });

            builder.Services.AddMudServices();

            await builder.Build().RunAsync();
        }
    }
}
