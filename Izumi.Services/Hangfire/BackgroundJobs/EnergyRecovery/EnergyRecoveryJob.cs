using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.EnergyRecovery
{
    public class EnergyRecoveryJob : IEnergyRecoveryJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EnergyRecoveryJob> _logger;
        private readonly AppDbContext _db;

        public EnergyRecoveryJob(
            IMediator mediator,
            ILogger<EnergyRecoveryJob> logger,
            DbContextOptions options)
        {
            _mediator = mediator;
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task Execute()
        {
            _logger.LogInformation(
                "Energy recovery job executed");

            var energyRecoveryNonPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.EnergyRecoveryNonPremium));
            var energyRecoveryPremium = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.EnergyRecoveryPremium));

            await _db.Database.ExecuteSqlInterpolatedAsync($@"
                update users
                set energy = (
                    case when energy + {energyRecoveryNonPremium} <= 100 then energy + {energyRecoveryNonPremium}
                         else 100
                    end),
                    updated_at = now()
                where is_premium = false;

                update users
                set energy = (
                    case when energy + {energyRecoveryPremium} <= 100 then energy + {energyRecoveryPremium}
                         else 100
                    end),
                    updated_at = now()
                where is_premium = true;");
        }
    }
}
