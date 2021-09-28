using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Data.Util;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadBuildingsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadBuildingsHandler : IRequestHandler<SeederUploadBuildingsCommand, TotalAndAffectedCountDto>
    {
        private readonly AppDbContext _db;

        public SeederUploadBuildingsHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadBuildingsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();

            foreach (var buildingType in Enum.GetValues(typeof(BuildingType)).Cast<BuildingType>())
            {
                result.Total++;

                var exist = await _db.Buildings
                    .AsQueryable()
                    .AnyAsync(x => x.Type == buildingType);

                if (exist) continue;

                try
                {
                    await _db.CreateEntity(new Building
                    {
                        Category = BuildingCategoryType.Personal,
                        Type = buildingType,
                        Name = buildingType.ToString(),
                        Description = "nothing here"
                    });

                    result.Affected++;
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }
    }
}
