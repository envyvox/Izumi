using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteExploreGarden
{
    public interface ICompleteExploreGardenJob
    {
        Task Execute(long userId);
    }
}
