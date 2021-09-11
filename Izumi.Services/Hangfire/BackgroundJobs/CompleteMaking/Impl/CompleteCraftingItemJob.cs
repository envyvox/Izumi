using System;
using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking.Impl
{
    public class CompleteCraftingItemJob : ICompleteCraftingItemJob
    {
        public async Task Execute(long userId, Guid craftingId, uint amount)
        {
            throw new NotImplementedException();
        }
    }
}
