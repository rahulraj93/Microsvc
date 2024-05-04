using AutoMapper;
using Microsvc.Services.ProductAPI.Models;
using Microsvc.Services.ProductAPI.Models.Dto;

namespace Micorsvc.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mapconfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().ReverseMap();
            });
            return mapconfig;
        }
    }
}
