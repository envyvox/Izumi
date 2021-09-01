using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Currency.Models
{
    public record UserCurrencyDto(
        CurrencyType Currency,
        uint Amount);

    public class UserCurrencyProfile : Profile
    {
        public UserCurrencyProfile() => CreateMap<UserCurrency, UserCurrencyDto>();
    }
}
