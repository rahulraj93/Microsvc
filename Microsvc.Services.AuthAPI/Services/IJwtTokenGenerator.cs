using Microsvc.Services.AuthAPI.Models;

namespace Microsvc.Services.AuthAPI.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateJwtToken(ApplicationUser token);
    }
}
