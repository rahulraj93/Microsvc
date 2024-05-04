using Micorsvc.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsvc.Web.Models;
using Microsvc.Web.Services.IServices;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsvc.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			this._productService = productService;
		}

		public async Task<IActionResult> ProductIndex()
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

		public async Task<IActionResult> ProductCreate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ProductCreate(ProductDto productDto)
		{
			if (ModelState.IsValid)
			{
				ResponseDto? response = await _productService.AddProductAsync(productDto);
				if (response != null && response.IsSuccess == true)
				{
					TempData["success"] = "Product Created";
					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			}
			return View();
		}

		public async Task<IActionResult> ProductDelete(int productId)
		{
			ResponseDto? response = await _productService.GetProductAsync(productId);

			if (response != null && response.IsSuccess)
			{
				ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(productDto);
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> ProductDelete(ProductDto productDto)
		{
			ResponseDto? response = await _productService.DeleteProductAsync(productDto.ProductId);

			if (response != null && response.IsSuccess)
			{
				TempData["success"] = "Product Deleted";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(productDto);
		}


	}
}
