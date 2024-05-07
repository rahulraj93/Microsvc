using Microsvc.Services.ShoppingCartAPI.Models.Dto;

namespace Microsvc.Services.ShoppingCartAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
