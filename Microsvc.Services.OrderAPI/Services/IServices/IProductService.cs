using Microsvc.Services.OrderAPI.Models.Dto;

namespace Microsvc.Services.OrderAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
