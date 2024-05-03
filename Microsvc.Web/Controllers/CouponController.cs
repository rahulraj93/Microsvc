using Micorsvc.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsvc.Web.Models;
using Microsvc.Web.Services.IServices;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsvc.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            this._couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? List = new();
            ResponseDto response = await _couponService.GetAllCouponAsync();

            if (response != null && response.IsSuccess)
            {
                List = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(List);
        }

		public async Task<IActionResult> CouponCreate()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto coupon)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.AddCouponAsync(coupon);
                if(response != null && response.IsSuccess == true)
                {
                    TempData["success"] = "Coupon Created";
                    return RedirectToAction(nameof(CouponIndex));                    
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View();
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

            if (response != null && response.IsSuccess)
            {
                CouponDto? coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(coupon);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();  
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Deleted";
                return RedirectToAction(nameof(CouponIndex));
                            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(couponDto);
        }


    }
}
