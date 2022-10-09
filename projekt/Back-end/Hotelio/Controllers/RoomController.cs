using Hotelio.Context;
using Hotelio.Data;
using Hotelio.Data.Enums;
using Hotelio.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private RoomService roomService;

        public RoomController(RoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet]
        [Route("getRooms")]
        public async Task<ActionResult<Room>> GetRooms()
        {
            return Ok(roomService.GetRooms());
        }

        [HttpGet]
        [Route("getRoom/{id}")]
        public async Task<ActionResult<Room>> GetRoom(Guid id)
        {
            var room = roomService.GetRoom(id);
            if (room != null)
            {
                return Ok(room);
            }

            return NotFound("No room with such Id exists");
        }

        [HttpPost]
        [Route("addRoom")]
        public async Task<ActionResult<Room>> AddRoom(Guid hotelId, Room room)
        {
            var roomToBeAdded = roomService.AddRoom(hotelId, room);
            if (roomToBeAdded == null)
            {
                return BadRequest();
            }

            return Created(HttpContext.Request.Scheme, room);
        }

        [HttpDelete]
        [Route("deleteRoom/{id}")]
        public async Task<ActionResult<Room>> DeleteRoom(Guid id)
        {
            var room = roomService.GetRoom(id);
            if (room != null)
            {
                roomService.DeleteRoom(room);
                return NoContent();
            }

            return NotFound($"Room with ID {id} not found");
        }

        [HttpPut]
        [Route("updateRoom/{id}")]
        public async Task<ActionResult<Room>> UpdateRoom(Guid id, Room room)
        {
            var existingRoom = roomService.GetRoom(id);
            if (existingRoom != null)
            {
                room.Id = existingRoom.Id;
                roomService.EditRoom(room);
                return Ok(existingRoom);
            }

            return NotFound($"Room with ID {id} not found");
        }
    }
}
