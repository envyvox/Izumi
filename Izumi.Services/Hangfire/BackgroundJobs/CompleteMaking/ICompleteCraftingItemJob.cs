using System;
using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking
{
    public interface ICompleteCraftingItemJob
    {
        Task Execute(long userId, Guid craftingId, uint amount);
    }
}
