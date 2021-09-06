﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.World.Queries
{
    public record GetWorldPropertyValueQuery(WorldPropertyType Type) : IRequest<uint>;

    public class GetWorldPropertyValueHandler : IRequestHandler<GetWorldPropertyValueQuery, uint>
    {
        private readonly AppDbContext _db;

        public GetWorldPropertyValueHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<uint> Handle(GetWorldPropertyValueQuery request, CancellationToken ct)
        {
            var entity = await _db.WorldProperties
                .SingleOrDefaultAsync(x => x.Type == request.Type);

            if (entity is null)
            {
                throw new Exception($"world property {request.Type.ToString()} doesnt exist");
            }

            return entity.Value;
        }
    }
}
