using System;
using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Hangfire.Models
{
    public record UserHangfireJobDto(
        HangfireJobType Type,
        string JobId,
        DateTimeOffset CreatedAt,
        DateTimeOffset Expiration);

    public class UserHangfireJobProfile : Profile
    {
        public UserHangfireJobProfile() => CreateMap<UserHangfireJob, UserHangfireJobDto>();
    }
}
