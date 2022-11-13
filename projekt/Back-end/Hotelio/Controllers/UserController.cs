using Hotelio.Auth;
using Hotelio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{
    [ApiController]
    [Authorize(Roles = HotelRoles.Owner)]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.Get(id);
            return user == null ? NotFound($"User with specified id = {id} was not found") : Ok(user);
        }
    }
}
