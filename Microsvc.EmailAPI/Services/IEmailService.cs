using Microsvc.Services.EmailAPI.Models.Dto;

namespace Microsvc.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailAndLog(string email);
    }
}
