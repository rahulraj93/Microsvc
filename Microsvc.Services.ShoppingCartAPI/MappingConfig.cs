using AutoMapper;
using Microsvc.Services.ShoppingCartAPI.Models;
using Microsvc.Services.ShoppingCartAPI.Models.Dto;

namespace Micorsvc.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mapconfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();;
            });
            return mapconfig;
        }
    }
}
