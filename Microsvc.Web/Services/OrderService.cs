using Micorsvc.Web.Models;
using Microsvc.Web.Models;
using Microsvc.Web.Services.IServices;
using Microsvc.Web.Utility;
using static Microsvc.Web.Utility.SD;

namespace Microsvc.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService )
        {
            this._baseService = baseService;
        }

        public async Task<ResponseDto?> CreateOrderAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = SD.OrderAPIBase + "/api/order/createorder"
            });
        }

        public async Task<ResponseDto?> CreateStripeSessionAsync(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.OrderAPIBase + "/api/order/createstripesession"
            });
        }
    }
}
