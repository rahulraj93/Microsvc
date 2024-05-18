using Micorsvc.Web.Models;
using Microsvc.Web.Models;

namespace Microsvc.Web.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDto?> CreateOrderAsync(CartDto cartDto);
        Task<ResponseDto?> CreateStripeSessionAsync(StripeRequestDto stripeRequestDto);
    }
}
