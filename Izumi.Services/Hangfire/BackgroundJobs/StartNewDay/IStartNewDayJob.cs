using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.StartNewDay
{
    public interface IStartNewDayJob
    {
        Task Execute();
    }
}
