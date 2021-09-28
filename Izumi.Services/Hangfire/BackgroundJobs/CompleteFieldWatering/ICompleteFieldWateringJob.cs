using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteFieldWatering
{
    public interface ICompleteFieldWateringJob
    {
        Task Execute(long userId);
    }
}
