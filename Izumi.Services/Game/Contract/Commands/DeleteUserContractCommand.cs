using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Contract.Commands
{
    public record DeleteUserContractCommand(long UserId) : IRequest;

    public class DeleteUserContractHandler : IRequestHandler<DeleteUserContractCommand>
    {
        private readonly ILogger<DeleteUserContractHandler> _logger;
        private readonly AppDbContext _db;

        public DeleteUserContractHandler(
            DbContextOptions options,
            ILogger<DeleteUserContractHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteUserContractCommand request, CancellationToken ct)
        {
            var entity = await _db.UserContracts
                .SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt work on contract");
            }

            await _db.DeleteEntity(entity);

            _logger.LogInformation(
                "Deleted user contract entity for user {UserId}",
                request.UserId);

            return Unit.Value;
        }
    }
}
