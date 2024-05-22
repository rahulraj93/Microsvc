using AutoMapper;
using Micorsvc.Services.RewardAPI.Models;
using Micorsvc.Services.RewardAPI.Models.Dto;

namespace Micorsvc.Services.RewardAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mapconfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Rewards, RewardsDto>();
            });
            return mapconfig;
        }
    }
}
