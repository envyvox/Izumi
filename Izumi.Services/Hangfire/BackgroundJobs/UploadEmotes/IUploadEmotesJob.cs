using System.Threading.Tasks;

namespace Izumi.Services.Hangfire.BackgroundJobs.UploadEmotes
{
    public interface IUploadEmotesJob
    {
        Task Execute();
    }
}
