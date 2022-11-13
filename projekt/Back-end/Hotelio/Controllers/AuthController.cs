using Hotelio.Auth;
using Hotelio.Auth.Model;
using Hotelio.Data;
using Hotelio.Data.Routes;
using Hotelio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenManager _tokenManager;

        public AuthController(UserManager<User> userManager, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByEmailAsync(registerUserDto.Email);

            if (user != null)
                return BadRequest("Vartotojas su tokiu pastu jau egzistuoja");

            var newUser = new User()
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);

            if (!createUserResult.Succeeded)
                return BadRequest("Ivyko klaida");

            await _userManager.AddToRoleAsync(newUser, HotelRoles.Client);

            return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.Email));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return BadRequest("Neteisingas slapyvardis arba slaptazodis");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.password);

            if(!isPasswordValid)
                return BadRequest("Neteisingas slapyvardis arba slaptazodis");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = await _tokenManager.CreateAccessTokenAsync(user);

            return Ok(new SuccessfulLoginDto(accessToken));
        }
    }
}
