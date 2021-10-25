using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.VoiceStatistic
{
    public interface IVoiceStatisticJob
    {
        Task Execute();
    }
}
