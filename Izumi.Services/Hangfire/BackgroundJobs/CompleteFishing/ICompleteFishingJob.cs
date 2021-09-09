using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteFishing
{
    public interface ICompleteFishingJob
    {
        Task Execute(long userId);
    }
}
