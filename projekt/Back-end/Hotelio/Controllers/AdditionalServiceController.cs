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

        [HttpGet]
        [Route(Routes.GetAdditionalServices)]
        [Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<AdditionalService>> GetAdditionalServices()
        {
            return Ok(additionalServiceService.GetAServices());
        }

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
