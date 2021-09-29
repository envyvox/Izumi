using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Contract.Commands
{
    public record CreateUserContractCommand(long UserId, Guid ContractId, DateTimeOffset Expiration) : IRequest;

    public class CreateUserContractHandler : IRequestHandler<CreateUserContractCommand>
    {
        private readonly ILogger<CreateUserContractHandler> _logger;
        private readonly AppDbContext _db;

        public CreateUserContractHandler(
            DbContextOptions options,
            ILogger<CreateUserContractHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUserContractCommand request, CancellationToken ct)
        {
            var exist = await _db.UserContracts
                .AnyAsync(x => x.UserId == request.UserId);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already work on contract");
            }

            await _db.CreateEntity(new UserContract
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ContractId = request.ContractId,
                Expiration = request.Expiration,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user contract entity for user {UserId} with contract {ContractId} and expiration {Expiration}",
                request.UserId, request.ContractId, request.Expiration);

            return Unit.Value;
        }
    }
}
