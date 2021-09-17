using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.User.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.User.Commands
{
    public record UpdateUserCommand(
            long Id,
            string About,
            TitleType Title,
            GenderType Gender,
            LocationType Location,
            uint Energy,
            uint Points,
            bool IsPremium,
            string CommandColor)
        : IRequest<UserDto>;

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateUserHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _mapper = mapper;
            _db = new AppDbContext(options);
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.Users
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.About = request.About;
            entity.Title = request.Title;
            entity.Gender = request.Gender;
            entity.Location = request.Location;
            entity.Energy = request.Energy;
            entity.Points = request.Points;
            entity.IsPremium = request.IsPremium;
            entity.CommandColor = request.CommandColor;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return _mapper.Map<UserDto>(entity);
        }
    }
}
