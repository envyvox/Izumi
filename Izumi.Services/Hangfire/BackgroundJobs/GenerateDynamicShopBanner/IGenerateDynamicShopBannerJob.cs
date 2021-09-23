using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.GenerateDynamicShopBanner
{
    public interface IGenerateDynamicShopBannerJob
    {
        Task Execute();
    }
}
