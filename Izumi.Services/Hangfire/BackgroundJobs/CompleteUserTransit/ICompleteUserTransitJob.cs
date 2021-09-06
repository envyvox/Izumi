using System.Threading.Tasks;
using Izumi.Data.Enums;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteUserTransit
{
    public interface ICompleteUserTransitJob
    {
        Task Execute(long userId, LocationType destination);
    }
}
