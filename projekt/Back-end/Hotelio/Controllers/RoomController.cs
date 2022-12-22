using Hotelio.Auth;
using Hotelio.Context;
using Hotelio.Data;
using Hotelio.Data.Enums;
using Hotelio.Data.Routes;
using Hotelio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelio.Controllers
{

    [ApiController]
    [EnableCors("CorsApi")]
    //[Authorize]
    public class RoomController : ControllerBase
    {
        private RoomService roomService;

        public RoomController(RoomService roomService)
        {
            this.roomService = roomService;
        }

        /// <summary>
        /// Gets all rooms
        /// </summary>
        /// <returns>List of all rooms</returns>
        /// <response code="200">List of all rooms returned</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        [HttpGet]
        [Route(Routes.GetRooms)]
        //[Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<Room>> GetRooms()
        {
            return Ok(roomService.GetRooms());
        }

        /// <summary>
        /// Fetches a specific room
        /// </summary>
        /// <param name="id">room's id</param>
        /// <returns>The found room, fail otherwise</returns>
        /// <response code="200">Found room</response>
        /// <response code="200">User is not authorized to perform this action</response>
        /// <response code="404">room with specified id was not found</response>
        [HttpGet]
        [Route(Routes.GetRoom)]
        //[Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<Room>> GetRoom(Guid roomId)
        {
            var room = roomService.GetRoom(roomId);
            if (room != null)
            {
                return Ok(room);
            }

            return NotFound("No room with such Id exists");
        }

        /// <summary>
        /// Creates a new room
        /// </summary>
        /// <returns>Success or error message</returns>
        /// <response code="201">room created</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Dormitory with specified id was not found</response>
        /// <response code="409">Room with specified number already exists</response>
        [HttpPost]
        [Route(Routes.AddRoom)]
        //[Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
        public async Task<ActionResult<Room>> AddRoom(Guid hotelId, Room room)
        {
            var roomToBeAdded = roomService.AddRoom(hotelId, room);
            if (roomToBeAdded == null)
            {
                return BadRequest();
            }

            return Created(HttpContext.Request.Scheme, room);
        }


        /// <summary>
        /// Deletes a specified room
        /// </summary>
        /// <param name="id">Room's id</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">Room was deleted successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">Room with specified id was not found</response>
        [HttpDelete]
        [Route(Routes.DeleteRoom)]
        //[Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
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

        /// <summary>
        /// Updates a specific room's properties
        /// </summary>
        /// <returns>Success or error message</returns>
        /// <response code="200">room was updated successfully</response>
        /// <response code="401">User is unauthorized to perform this action</response>
        /// <response code="404">room with specified id was not found</response>
        [HttpPut]
        [Route(Routes.UpdateRoom)]
        //[Authorize(Roles = $"{HotelRoles.Owner}, {HotelRoles.Admin}")]
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
