using Micorsvc.Services.ShoppingCartAPI.Models.Dto;
using Microsvc.Services.ShoppingCartAPI.Services.IServices;
using Newtonsoft.Json;

namespace Microsvc.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponId)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponId}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(resp.Result.ToString());
            }
            return new CouponDto();
        }
    }
}
