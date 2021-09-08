using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Collection.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Collection.Queries
{
    public record GetUserCollectionsQuery(long UserId, CollectionType Type) : IRequest<List<UserCollectionDto>>;

    public class GetUserCollectionsHandler : IRequestHandler<GetUserCollectionsQuery, List<UserCollectionDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserCollectionsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserCollectionDto>> Handle(GetUserCollectionsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserCollections
                .AsQueryable()
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type)
                .ToListAsync();

            return _mapper.Map<List<UserCollectionDto>>(entities);
        }
    }
}
