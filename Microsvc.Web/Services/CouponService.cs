using Micorsvc.Web.Models;
using Microsvc.Web.Models;
using Microsvc.Web.Services.IServices;
using Microsvc.Web.Utility;
using static Microsvc.Web.Utility.SD;

namespace Microsvc.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService )
        {
            this._baseService = baseService;
        }

        public async Task<ResponseDto?> AddCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Data = couponDto,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = SD.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/" + couponId
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto?> UpdateCouponByAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.PUT,
                Data = couponDto,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }
    }
}
