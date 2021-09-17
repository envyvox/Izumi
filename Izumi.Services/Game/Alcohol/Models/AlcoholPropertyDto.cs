using AutoMapper;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Alcohol.Models
{
    public record AlcoholPropertyDto(
        Data.Entities.Resource.Alcohol Alcohol,
        AlcoholPropertyType Property,
        uint Value);

    public class AlcoholPropertyProfile : Profile
    {
        public AlcoholPropertyProfile() => CreateMap<AlcoholProperty, AlcoholPropertyDto>();
    }
}
