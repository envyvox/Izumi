using System;
using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking
{
    public interface ICompleteCraftingAlcoholJob
    {
        Task Execute(long userId, Guid alcoholId, uint amount);
    }
}
