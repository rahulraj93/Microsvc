using AutoMapper;
using Microsvc.Services.OrderAPI.Models;
using Microsvc.Services.OrderAPI.Models.Dto;

namespace Micorsvc.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mapconfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(d=>d.CartTotal, u=>u.MapFrom(s=>s.OrderTotal)).ReverseMap();

                config.CreateMap<CartDetailsDto, OrderDetailsDto>()
               .ForMember(d => d.ProductName, u => u.MapFrom(s => s.Product.Name))
               .ForMember(d => d.Price, u => u.MapFrom(s => s.Product.Price));

                config.CreateMap<OrderDetailsDto, CartDetailsDto>();

                config.CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            });
            return mapconfig;
        }
    }
}
