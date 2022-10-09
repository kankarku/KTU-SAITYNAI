using Hotelio.Data;
using Hotelio.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalServiceController : ControllerBase
    {
        private AdditionalServiceService additionalServiceService;

        public AdditionalServiceController(AdditionalServiceService additionalServiceService)
        {
            this.additionalServiceService = additionalServiceService;
        }

        [HttpGet]
        [Route("getAdditionalServices")]
        public async Task<ActionResult<AdditionalService>> GetAdditionalServices()
        {
            return Ok(additionalServiceService.GetAServices());
        }

        [HttpGet]
        [Route("getAdditionalService/{id}")]
        public async Task<ActionResult<AdditionalService>> GetAdditionalService(Guid id)
        {
            var additionalService = additionalServiceService.GetAservice(id);
            if (additionalService != null)
            {
                return Ok(additionalService);
            }

            return NotFound("No additional service with such Id exists");
        }

        [HttpPost]
        [Route("addAdditionalService")]
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
        [Route("deleteAdditionalService/{id}")]
        public async Task<ActionResult<AdditionalService>> DeleteAdditionalService(Guid id)
        {
            var additionalService = additionalServiceService.GetAservice(id);
            if (additionalService != null)
            {
                additionalServiceService.DeleteAservice(additionalService);
                return NoContent();
            }

            return NotFound($"Room with ID {id} not found");
        }

        [HttpPut]
        [Route("updateAdditionalService/{id}")]
        public async Task<ActionResult<AdditionalService>> UpdateAdditionalService(Guid id, AdditionalService additionalService)
        {
            var existingService = additionalServiceService.GetAservice(id);
            if (existingService != null)
            {
                additionalService.Id = existingService.Id;
                additionalServiceService.EditAservice(additionalService);
                return Ok(existingService);
            }

            return NotFound($"Room with ID {id} not found");
        }
    }
}
