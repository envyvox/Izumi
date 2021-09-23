using System;
using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking.Impl
{
    public class CompleteCraftingDrinkJob : ICompleteCraftingDrinkJob
    {
        public async Task Execute(long userId, Guid drinkId, uint amount)
        {
            // todo implement
            await Task.CompletedTask;
        }
    }
}
