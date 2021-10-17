using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.Unmute
{
    public interface IUnmuteJob
    {
        Task Execute(long userId);
    }
}
