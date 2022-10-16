using Hotelio.Data;
using Hotelio.Data.Routes;
using Hotelio.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{
    [ApiController]
    public class AdditionalServiceController : ControllerBase
    {
        private AdditionalServiceService additionalServiceService;

        public AdditionalServiceController(AdditionalServiceService additionalServiceService)
        {
            this.additionalServiceService = additionalServiceService;
        }

        [HttpGet]
        [Route(Routes.GetAdditionalServices)]
        public async Task<ActionResult<AdditionalService>> GetAdditionalServices()
        {
            return Ok(additionalServiceService.GetAServices());
        }

        [HttpGet]
        [Route(Routes.GetAdditionalService)]
        public async Task<ActionResult<AdditionalService>> GetAdditionalService(Guid serviceId)
        {
            var additionalService = additionalServiceService.GetAservice(serviceId);
            if (additionalService != null)
            {
                return Ok(additionalService);
            }

            return NotFound("No additional service with such Id exists");
        }

        [HttpPost]
        [Route(Routes.AddAdditionalService)]
        public async Task<ActionResult<AdditionalService>> AddAdditionalService(Guid roomId, AdditionalService additionalService)
        {
            var serviceToBeAdded = additionalServiceService.AddAservice(roomId, additionalService);
            if (serviceToBeAdded == null)
            {
                return BadRequest();
            }

            return Created(HttpContext.Request.Scheme, additionalService);
        }

        [HttpDelete]
        [Route(Routes.DeleteAdditionalService)]
        public async Task<ActionResult<AdditionalService>> DeleteAdditionalService(Guid serviceId)
        {
            var additionalService = additionalServiceService.GetAservice(serviceId);
            if (additionalService != null)
            {
                additionalServiceService.DeleteAservice(additionalService);
                return NoContent();
            }

            return NotFound($"Room with ID {serviceId} not found");
        }

        [HttpPut]
        [Route(Routes.UpdateAdditionalService)]
        public async Task<ActionResult<AdditionalService>> UpdateAdditionalService(Guid serviceId, AdditionalService additionalService)
        {
            var existingService = additionalServiceService.GetAservice(serviceId);
            if (existingService != null)
            {
                additionalService.Id = existingService.Id;
                additionalServiceService.EditAservice(additionalService);
                return Ok(existingService);
            }

            return NotFound($"Room with ID {serviceId} not found");
        }
    }
}
