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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            IJwtTokenGenerator jwtTokenGenerator)
        {
            this._db = db;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToUpper() == loginRequestDto.Username.ToUpper());

            var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);  

            if (!isValid || user == null)
            {
                return new LoginResponseDto { User = null, Token = "" };
            }

            //Token generation
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateJwtToken(user, roles);

            UserDto userDto = new()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            return new LoginResponseDto
            {
                User = userDto,
                Token = token
            };

        }

        public async Task<string> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user,registrationRequestDto.Password);
                if(result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(x=>x.UserName==registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }
    }
}
