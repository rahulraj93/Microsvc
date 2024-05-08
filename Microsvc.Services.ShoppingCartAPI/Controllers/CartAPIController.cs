using AutoMapper;
using Micorsvc.Services.ShoppingCartAPI.Data;
using Micorsvc.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsvc.MessageBus;
using Microsvc.Services.ShoppingCartAPI.Models;
using Microsvc.Services.ShoppingCartAPI.Models.Dto;
using Microsvc.Services.ShoppingCartAPI.Services.IServices;
using System.Reflection.PortableExecutable;

namespace Microsvc.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private ResponseDto _response;

        public CartAPIController(AppDbContext db
            ,IMapper mapper
            ,IProductService productService
            ,ICouponService couponService
            ,IMessageBus messageBus
            ,IConfiguration configuration)
        {
            this._db = db;            
            this._productService = productService;
            this._couponService = couponService;
            this._messageBus = messageBus;
            this._configuration = configuration;
            _mapper = mapper;
            _response = new ResponseDto();
        }
        [HttpPost("cartupsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;

        }

        [HttpPost("removecart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails
                   .First(u => u.CartDetailsId == cartDetailsId);

                int totalCountofCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if (totalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders
                       .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpGet("getcart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try 
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(u => u.UserId == userId))
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails
                    .Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                var productDto = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDto.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }
                //applycoupon
                if(!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }
                _response.Result = cart;
            }
            catch(Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("applycoupon")]
        public async Task<object> ApplyCoupon([FromBody]CartDto cartDto)
        {
            try
            {
                var cartHeader = await _db.CartHeaders.FirstAsync(x=>x.UserId == cartDto.CartHeader.UserId);   
                cartHeader.CouponCode = cartDto.CartHeader.CouponCode;
                _db.CartHeaders.Update(cartHeader);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("removecoupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartHeader = await _db.CartHeaders.FirstAsync(x => x.UserId == cartDto.CartHeader.UserId);
                cartHeader.CouponCode = "";
                _db.CartHeaders.Update(cartHeader);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("emailcart")]
        public async Task<object> EmailCartRequest([FromBody] CartDto cartDto)
        {
            try
            {
                var topicQueueName = _configuration.GetValue<string>("TopicAndQueue:EmailCartQueue");
                await _messageBus.PublishMessage(cartDto, topicQueueName);
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

    }
}
