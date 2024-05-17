using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsvc.Services.OrderAPI.Services.IServices;
using Micorsvc.Services.OrderAPI.Data;
using Microsvc.Services.OrderAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsvc.Services.OrderAPI.Utility;
using Microsvc.Services.OrderAPI.Models;

namespace Microsvc.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;
        private readonly IProductService _productService;
        private ResponseDto _response;

        public OrderAPIController(AppDbContext db
            , IMapper mapper
            , IProductService productService)
        {
            this._db = db;
            this._productService = productService;
            _mapper = mapper;
            _response = new ResponseDto();
        }
        [Authorize]
        [HttpPost("createorder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

                OrderHeader orderCreated = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;

                await _db.SaveChangesAsync();
                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

    }
}
