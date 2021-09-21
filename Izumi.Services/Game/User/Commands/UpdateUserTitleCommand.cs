﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.User.Commands
{
    public record UpdateUserTitleCommand(long UserId, TitleType Title) : IRequest;

    public class UpdateUserTitleHandler : IRequestHandler<UpdateUserTitleCommand>
    {
        private readonly ILogger<UpdateUserTitleHandler> _logger;
        private readonly AppDbContext _db;

        public UpdateUserTitleHandler(
            DbContextOptions options,
            ILogger<UpdateUserTitleHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateUserTitleCommand request, CancellationToken ct)
        {
            var entity = await _db.Users
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            entity.Title = request.Title;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Updated user {UserId} title to {Title}",
                request.UserId, request.Title.ToString());

            return Unit.Value;
        }
    }
}
