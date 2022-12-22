using System.Security.Claims;
using Hotelio.Auth;
using Hotelio.Data;
using Hotelio.Data.Routes;
using Hotelio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Hotelio.Controllers
{
    [ApiController]
    [EnableCors("CorsApi")]
    [Authorize]
    public class AdditionalServiceController : ControllerBase
    {
        private AdditionalServiceService additionalServiceService;

        public AdditionalServiceController(AdditionalServiceService additionalServiceService)
        {
            this.additionalServiceService = additionalServiceService;
        }

        /// <summary>
        /// Gets all aa and their data
        /// </summary>
        /// <returns>List of all aa</returns>
        /// <response code="200">List of all aa</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        [HttpGet]
        [Route(Routes.GetAdditionalServices)]
        [Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<AdditionalService>> GetAdditionalServices()
        {
            return Ok(additionalServiceService.GetAServices());
        }

        /// <summary>
        /// Gets a specific aa
        /// </summary>
        /// <param name="id">Aa's id</param>
        /// <returns>Found aa or error message</returns>
        /// <response code="200">Found aa</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Aa with specified id was not found</response>
        [HttpGet]
        [Route(Routes.GetAdditionalService)]
        public async Task<ActionResult<AdditionalService>> GetAdditionalService(Guid serviceId)
        {
            var additionalService = await additionalServiceService.GetAservice(User, serviceId);

            if (additionalService != null)
            {
                return Ok(additionalService);
            }

            return NotFound("No additional service with such Id exists");
        }


        /// <summary>
        /// Creates a new aa
        /// </summary>
        /// <returns>Success or error message</returns>
        /// <response code="201">aa created successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="409">aa with specified number already exists</response>
        [HttpPost]
        [Route(Routes.AddAdditionalService)]
        [Authorize(Roles = HotelRoles.Client)]
        public async Task<ActionResult<AdditionalService>> AddAdditionalService(Guid roomId, AdditionalService additionalService)
        {
            //var serviceToBeAdded = additionalServiceService.AddAservice(roomId, additionalService);
            //additionalService.UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            //if (serviceToBeAdded == null)
            //{
            //    return BadRequest();
            //}

            //return Created(HttpContext.Request.Scheme, additionalService);

            return additionalServiceService.AddAservice(User.FindFirstValue(JwtRegisteredClaimNames.Sub), roomId, additionalService) != null
                ? CreatedAtAction(nameof(AddAdditionalService), "Additional Service created.")
                : BadRequest($"User with id = {User.FindFirstValue(JwtRegisteredClaimNames.Sub)} was not found");
        }

        /// <summary>
        /// Deletes a specific aa
        /// </summary>
        /// <param name="id">aa id to delete</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">aa deleted successfully</response>
        /// <response code="404">aa with specified id was not found</response>
        [HttpDelete]
        [Route(Routes.DeleteAdditionalService)]
        [Authorize(Roles = HotelRoles.Client)]
        public async Task<ActionResult<AdditionalService>> DeleteAdditionalService(Guid serviceId)
        {
            var additionalService = await additionalServiceService.GetAservice(User,serviceId);
            if (additionalService != null)
            {
                additionalServiceService.DeleteAservice(additionalService);
                return NoContent();
            }

            return NotFound($"Room with ID {serviceId} not found");
        }

        /// <summary>
        /// Updates a specified aa
        /// </summary>
        /// <returns>Success or error message</returns>
        /// <response code="200">aa updated successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="400">No parameters were used in API call</response>
        /// <response code="404">aa with specified id was not found</response>
        [HttpPut]
        [Route(Routes.UpdateAdditionalService)]
        [Authorize(Roles = HotelRoles.Client)]
        public async Task<ActionResult<AdditionalService>> UpdateAdditionalService(Guid serviceId, AdditionalService additionalService)
        {
            var existingService = await additionalServiceService.GetAservice(User, serviceId);
            if (existingService != null)
            {
                additionalService.Id = (existingService.Id);
                return await additionalServiceService.EditAservice(User, additionalService)
                    ? Ok()
                    : NotFound($"Room with ID {serviceId} not found");
                //return Ok(existingService);
            }

            return NotFound($"Room with ID {serviceId} not found");
        }
    }
}
