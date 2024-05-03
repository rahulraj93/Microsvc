using Microsvc.Services.AuthAPI.Models.Dto;

namespace Microsvc.Services.AuthAPI.Services
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
