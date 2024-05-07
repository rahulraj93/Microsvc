﻿using Microsvc.Web.Models;

namespace Microsvc.Web.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveCouponAsync(CartDto cartDto);
    }
}
