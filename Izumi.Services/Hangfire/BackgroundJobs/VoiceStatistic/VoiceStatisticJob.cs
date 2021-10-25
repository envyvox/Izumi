using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.VoiceStatistic
{
    public class VoiceStatisticJob : IVoiceStatisticJob
    {
        private readonly ILogger<VoiceStatisticJob> _logger;
        private readonly AppDbContext _db;

        public VoiceStatisticJob(
            DbContextOptions options,
            ILogger<VoiceStatisticJob> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task Execute()
        {
            _logger.LogInformation(
                "Voice statistic job executed");

            await _db.Database.ExecuteSqlInterpolatedAsync(@$"
                insert into user_statistics as us (id, type, amount, created_at, updated_at, user_id)
                select uuid_generate_v4(), {StatisticType.VoiceMinutes}, 1, now(), now(), user_id from user_voices
                on conflict (user_id, type) do update set amount = us.amount + 1;");
        }
    }
}
