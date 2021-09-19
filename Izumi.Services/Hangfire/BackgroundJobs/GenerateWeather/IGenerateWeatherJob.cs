using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.GenerateWeather
{
    public interface IGenerateWeatherJob
    {
        Task Execute();
    }
}
