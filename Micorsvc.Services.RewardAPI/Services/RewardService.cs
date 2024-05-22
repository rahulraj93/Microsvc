using Micorsvc.Services.RewardAPI.Data;
using Micorsvc.Services.RewardAPI.Message;
using Micorsvc.Services.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsvc.Services.RewardAPI.Services;
using System.Text;

namespace Microsvc.Services.RewardAPI.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> _options;

        public RewardService(DbContextOptions<AppDbContext> options)
        {
            this._options = options;
        }

        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
            try
            {
                Rewards rewards = new()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardActivity = rewardsMessage.RewardsActivity,
                    UserId = rewardsMessage.UserId,
                    RewardDate = DateTime.Now
                };
                await using var _db = new AppDbContext(_options);
                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
