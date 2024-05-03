using Microsvc.Services.AuthAPI.Models.Dto;

namespace Microsvc.Services.AuthAPI.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
