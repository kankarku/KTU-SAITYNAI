using Hotelio.Context;
using Hotelio.Data;
using Hotelio.Data.Enums;
using Hotelio.Data.Routes;
using Hotelio.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{

    [ApiController]
    public class RoomController : ControllerBase
    {
        private RoomService roomService;

        public RoomController(RoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet]
        [Route(Routes.GetRooms)]
        public async Task<ActionResult<Room>> GetRooms()
        {
            return Ok(roomService.GetRooms());
        }

        [HttpGet]
        [Route(Routes.GetRoom)]
        public async Task<ActionResult<Room>> GetRoom(Guid roomId)
        {
            var room = roomService.GetRoom(roomId);
            if (room != null)
            {
                return Ok(room);
            }

            return NotFound("No room with such Id exists");
        }

        [HttpPost]
        [Route(Routes.AddRoom)]
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
        [Route(Routes.DeleteRoom)]
        public async Task<ActionResult<Room>> DeleteRoom(Guid roomId)
        {
            var room = roomService.GetRoom(roomId);
            if (room != null)
            {
                roomService.DeleteRoom(room);
                return NoContent();
            }

            return NotFound($"Room with ID {roomId} not found");
        }

        [HttpPut]
        [Route(Routes.UpdateRoom)]
        public async Task<ActionResult<Room>> UpdateRoom(Guid roomId, Room room)
        {
            var existingRoom = roomService.GetRoom(roomId);
            if (existingRoom != null)
            {
                room.Id = existingRoom.Id;
                roomService.EditRoom(room);
                return Ok(existingRoom);
            }

            return NotFound($"Room with ID {roomId} not found");
        }
    }
}
