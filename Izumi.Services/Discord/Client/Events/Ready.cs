using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Hangfire;
using Izumi.Services.Hangfire.BackgroundJobs.EnergyRecovery;
using Izumi.Services.Hangfire.BackgroundJobs.GenerateDynamicShopBanner;
using Izumi.Services.Hangfire.BackgroundJobs.StartNewDay;
using Izumi.Services.Hangfire.BackgroundJobs.UploadEmotes;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Client.Events
{
    public record Ready(DiscordSocketClient SocketClient) : IRequest;

    public class ReadyHandler : IRequestHandler<Ready>
    {
        private readonly ILogger<ReadyHandler> _logger;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly TimeZoneInfo _timeZoneInfo;

        public ReadyHandler(
            ILogger<ReadyHandler> logger,
            IHostApplicationLifetime lifetime,
            TimeZoneInfo timeZoneInfo)
        {
            _logger = logger;
            _lifetime = lifetime;
            _timeZoneInfo = timeZoneInfo;
        }

        public async Task<Unit> Handle(Ready request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Bot started");

                RecurringJob.AddOrUpdate<IUploadEmotesJob>("upload-emotes",
                    x => x.Execute(),
                    Cron.Hourly, _timeZoneInfo);
                RecurringJob.AddOrUpdate<IEnergyRecoveryJob>("recovery-energy",
                    x => x.Execute(),
                    Cron.Hourly, _timeZoneInfo);

                RecurringJob.AddOrUpdate<IStartNewDayJob>("start-new-day",
                    x => x.Execute(),
                    Cron.Daily, _timeZoneInfo);
                RecurringJob.AddOrUpdate<IGenerateDynamicShopBannerJob>("generate-dynamic-shop-banners",
                    x => x.Execute(),
                    Cron.Daily, _timeZoneInfo);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e,
                    "Unable to startup the bot. Application will now exit");

                _lifetime.StopApplication();
            }

            return await Unit.Task;
        }
    }
}
