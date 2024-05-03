using Micorsvc.Web.Models;
using Microsvc.Web.Models;

namespace Microsvc.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string couponId);
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> UpdateCouponByAsync(CouponDto couponDto);
        Task<ResponseDto?> AddCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
