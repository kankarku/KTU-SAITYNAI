using Hotelio.Context;
using Hotelio.Data;
using Hotelio.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private HotelService hotelService;

        public HotelController(HotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [HttpGet]
        [Route("getHotels")]
        public async Task<ActionResult<Hotel>> GetHotels()
        {
            return Ok(hotelService.GetHotels());
        }

        [HttpGet]
        [Route("getHotel/{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(Guid id)
        {
            var hotel = hotelService.GetHotel(id);
            if (hotel != null)
            {
                return Ok(hotel);
            }

            return NotFound("No hotel with such Id exists");
        }

        [HttpPost]
        [Route("addHotel")]
        public async Task<ActionResult<Hotel>> AddHotel(Hotel room)
        {
            hotelService.AddHotel(room);

            return Created(HttpContext.Request.Scheme, room);
        }

        [HttpDelete]
        [Route("deleteHotel/{id}")]
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

        [HttpPut]
        [Route("updateHotel/{id}")]
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
