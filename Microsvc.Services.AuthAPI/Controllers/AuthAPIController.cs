﻿using Micorsvc.Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsvc.MessageBus;
using Microsvc.Services.AuthAPI.Models.Dto;
using Microsvc.Services.AuthAPI.Services;

namespace Microsvc.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService
                                ,IMessageBus messageBus
                                ,IConfiguration configuration)
        {
            this._authService = authService;
            this._messageBus = messageBus;
            this._configuration = configuration;
            _response = new ResponseDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registration)
        {
            var errorMessage = await _authService.RegisterAsync(registration);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            await _messageBus.PublishMessage(registration.Email, _configuration.GetValue<string>("TopicAndQueue:RegistrationQueue"));
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.LoginAsync(loginRequestDto);
            if(loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username and Password is Invalid";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("assignrole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto requestDto)
        {
            var isRoleAssigned = await _authService.AssignRole(requestDto.Email, requestDto.Role);
            if (!isRoleAssigned)
            {
                _response.IsSuccess = false;
                _response.Message = "Error in role assign";
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
