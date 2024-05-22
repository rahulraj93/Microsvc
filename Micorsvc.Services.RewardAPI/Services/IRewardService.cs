
using Micorsvc.Services.RewardAPI.Message;

namespace Microsvc.Services.RewardAPI.Services
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
