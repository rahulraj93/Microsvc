using AutoMapper;
using Micorsvc.Services.CouponAPI.Models;
using Micorsvc.Services.CouponAPI.Models.Dto;

namespace Micorsvc.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mapconfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>();
                config.CreateMap<CouponDto, Coupon>();
            });
            return mapconfig;
        }
    }
}
