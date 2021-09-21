using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.EnergyRecovery
{
    public interface IEnergyRecoveryJob
    {
        Task Execute();
    }
}
