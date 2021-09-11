using System;
using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking
{
    public interface ICompleteCraftingDrinkJob
    {
        Task Execute(long userId, Guid drinkId, uint amount);
    }
}
