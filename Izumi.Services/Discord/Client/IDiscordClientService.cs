using System.Threading.Tasks;
using Discord.WebSocket;

namespace Izumi.Services.Discord.Client
{
    public interface IDiscordClientService
    {
        Task<DiscordSocketClient> GetSocketClient();
        Task Start();
    }
}
