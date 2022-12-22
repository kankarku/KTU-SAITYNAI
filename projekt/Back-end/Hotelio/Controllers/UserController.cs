using Hotelio.Auth;
using Hotelio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{
    [ApiController]
    [Authorize(Roles = HotelRoles.Owner)]
    [EnableCors("CorsApi")]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>List of all users</returns>
        /// <response code="200">List of all users</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetUsers());
        }

        /// <summary>
        /// Gets a specific user's information
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>A specific user's information</returns>
        /// <response code="200">Specific's user's info</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.Get(id);
            return user == null ? NotFound($"User with specified id = {id} was not found") : Ok(user);
        }
    }
}
