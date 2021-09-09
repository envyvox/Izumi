using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteExploreCastle
{
    public interface ICompleteExploreCastleJob
    {
        Task Execute(long userId);
    }
}
