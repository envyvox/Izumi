using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteContract
{
    public interface ICompleteContractJob
    {
        Task Execute(long userId, long contractIncId);
    }
}
