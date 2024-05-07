using Micorsvc.Services.ShoppingCartAPI.Models.Dto;

namespace Microsvc.Services.ShoppingCartAPI.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponId);
    }
}
