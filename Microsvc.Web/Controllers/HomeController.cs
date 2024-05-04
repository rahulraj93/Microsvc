using Micorsvc.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsvc.Web.Models;
using Microsvc.Web.Services.IServices;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Microsvc.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            this._productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto>? List = new();
            ResponseDto response = await _productService.GetAllProductAsync();

            if (response != null && response.IsSuccess)
            {
                List = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(List);
        }

        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto product = new();
            ResponseDto response = await _productService.GetProductAsync(productId);

            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
