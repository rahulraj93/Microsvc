using Micorsvc.Services.AuthAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsvc.Services.AuthAPI.Models;
using Microsvc.Services.AuthAPI.Models.Dto;

namespace Microsvc.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            this._db = db;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        public Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
