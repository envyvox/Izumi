using System;
using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking.Impl
{
    public class CompleteCraftingAlcoholJob : ICompleteCraftingAlcoholJob
    {
        public async Task Execute(long userId, Guid alcoholId, uint amount)
        {
            throw new NotImplementedException();
        }
    }
}
