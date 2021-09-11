using System;
using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking
{
    public interface ICompleteCookingJob
    {
        Task Execute(long userId, Guid foodId, uint amount);
    }
}
