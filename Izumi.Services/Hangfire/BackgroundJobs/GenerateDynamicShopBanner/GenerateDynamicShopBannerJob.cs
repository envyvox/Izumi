using System;
using System.Linq;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities;
using Izumi.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.GenerateDynamicShopBanner
{
    public class GenerateDynamicShopBannerJob : IGenerateDynamicShopBannerJob
    {
        private readonly ILogger<GenerateDynamicShopBannerJob> _logger;
        private readonly AppDbContext _db;

        public GenerateDynamicShopBannerJob(
            DbContextOptions options,
            ILogger<GenerateDynamicShopBannerJob> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task Execute()
        {
            _logger.LogInformation(
                "Generate dynamic shop banner job executed");

            await _db.Database.ExecuteSqlRawAsync("truncate dynamic_shop_banners;");

            var banners = await _db.Banners
                .OrderByRandom()
                .Where(x => x.AutoIncrementedId != 1) // ignore default banner
                .Take(6)
                .ToListAsync();

            foreach (var banner in banners)
            {
                await _db.CreateEntity(new DynamicShopBanner { Id = Guid.NewGuid(), BannerId = banner.Id });

                _logger.LogInformation(
                    "Created dynamic shop entity for banner {BannerId}",
                    banner.Id);
            }
        }
    }
}
