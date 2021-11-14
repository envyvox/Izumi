using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.GenerateDynamicShopRecipe
{
    public interface IGenerateDynamicShopRecipeJob
    {
        Task Execute();
    }
}