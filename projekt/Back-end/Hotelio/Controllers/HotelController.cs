using Hotelio.Auth;
using Hotelio.Context;
using Hotelio.Data;
using Hotelio.Data.Routes;
using Hotelio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsApi")]
    //[Authorize]
    public class HotelController : ControllerBase
    {
        private HotelService hotelService;

        public HotelController(HotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        /// <summary>
        /// Gets all available hotels.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Returns all hotels.
        /// </para>
        /// </remarks>
        /// <response code="200">List of hotels.</response>
        [HttpGet]
        [Route(Routes.GetHotels)]
        //[Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<Hotel>> GetHotels()
        {
            return Ok(hotelService.GetHotels());
        }

        /// <summary>
        /// Fetches a specific hotel.
        /// </summary>
        /// <param name="id">hotels's id.</param>
        /// <remarks>
        /// <para>
        /// Returns hotel with its information.
        /// </para>
        /// </remarks>
        /// <response code="200">Found hotel.</response>
        /// <response code="400">Hotel with specified id was not found.</response>
        [HttpGet]
        [Route(Routes.GetHotel)]
        //[Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<Hotel>> GetHotel(Guid id)
        {
            var hotel = hotelService.GetHotel(id);
            if (hotel != null)
            {
                return Ok(hotel);
            }

            return NotFound("No hotel with such Id exists");
        }

        /// <summary>
        /// Creates a new hotel.
        /// </summary>
        /// <para>
        /// Success/failure message
        /// </para>
        /// <response code="201">Hotel created successfully.</response>
        /// <response code="400">Invalid parameters were entered.</response>
        /// <response code="401">User is unauthorized to perform this action.</response>
        /// <response code="409">Hotel with the same name already exists.</response>
        [HttpPost]
        [Route(Routes.AddHotel)]
       // [Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<Hotel>> AddHotel(Hotel room)
        {
            hotelService.AddHotel(room);

            return Created(HttpContext.Request.Scheme, room);
        }

        /// <summary>
        /// Deletes a specific hotel.
        /// </summary>
        /// <param name="id">Hotel id</param>
        /// <returns>Success/failure message</returns>
        /// <response code="200">Hotel deleted successfully</response>
        /// <response code="401">User is unauthorized to perform this action.</response>
        /// <response code="404">Hotel not found </response>
        [HttpDelete]
        [Route(Routes.DeleteHotel)]
       // [Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<Hotel>> DeleteHotel(Guid id)
        {
            var hotel = hotelService.GetHotel(id);
            if (hotel != null)
            {
                hotelService.DeleteHotel(hotel);
                return NoContent();
            }

            return NotFound($"Hotel with ID {id} not found");
        }

        /// <summary>
        /// Updates a specific hotel.
        /// </summary>
        /// <returns>Success/failure message</returns>
        /// <response code="200">Hotel updated successfully</response>
        /// <response code="400">No parameters were entered</response>
        /// <response code="401">User is unauthorized to perform this action.</response>
        /// <response code="404">Hotel not found </response>
        [HttpPut]
        [Route(Routes.UpdateHotel)]
      //  [Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<Hotel>> UpdateHotel(Guid id, Hotel hotel)
        {
            var existingHotel = hotelService.GetHotel(id);
            if (existingHotel != null)
            {
                hotel.Id = existingHotel.Id;
                hotelService.EditHotel(hotel);
                return Ok(existingHotel);
            }

            return NotFound($"Hotel with ID {id} not found");
        }
    }
}
