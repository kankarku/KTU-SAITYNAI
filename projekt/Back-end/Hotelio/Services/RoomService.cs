using Hotelio.Context;
using Hotelio.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Services
{
    public class RoomService
    {
        private CrudContext crudContext;
        public RoomService(CrudContext crudContext)
        {
            this.crudContext = crudContext;
        }

        public Room GetRoom(Guid id)
        {
            return crudContext.Rooms
                .Include(x => x.Hotel)
                .SingleOrDefault(x => x.Id == id);
        }

        public List<Room> GetRooms()
        {
            return crudContext.Rooms
                .Include(x => x.Hotel)
                .ToList();
        }
        
        public void DeleteRoom(Room room)
        {
            crudContext.Rooms.Remove(room);
            crudContext.SaveChanges();
        }

        public Room AddRoom(Guid HotelId, Room room)
        {
            var hotel = crudContext.Hotels.Find(HotelId);
            if (hotel == null) return null;

            room.Hotel = hotel;
            room.Id = Guid.NewGuid();
            crudContext.Rooms.Add(room);
            crudContext.SaveChanges();
            return room;
        }

        public Room EditRoom(Room room)
        {
            var existingRoom = crudContext.Rooms.Find(room.Id);
            if (existingRoom != null)
            {
                existingRoom.RoomLevel = room.RoomLevel;
                existingRoom.RoomType = room.RoomType;
                crudContext.Rooms.Update(existingRoom);
                crudContext.SaveChanges();
            }

            return room;
        }
    }
}
