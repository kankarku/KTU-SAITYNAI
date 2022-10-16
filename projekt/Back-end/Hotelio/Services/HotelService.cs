using Hotelio.Context;
using Hotelio.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Services
{
    public class HotelService
    {
        private CrudContext crudContext;
        public HotelService(CrudContext crudContext)
        {
            this.crudContext = crudContext;
        }

        public Hotel GetHotel(Guid id)
        {
            return crudContext.Hotels
                .SingleOrDefault(x => x.Id == id);
        }

        public List<Hotel> GetHotels()
        {
            return crudContext.Hotels
                .ToList();
        }

        public Hotel AddHotel(Hotel hotel)
        {
            hotel.Id = Guid.NewGuid();
            crudContext.Hotels.Add(hotel);
            crudContext.SaveChanges();
            return hotel;
        }

        public void DeleteHotel(Hotel hotel)
        {
            crudContext.Hotels.Remove(hotel);
            crudContext.SaveChanges();
        }

        public Hotel EditHotel(Hotel hotel)
        {
            var existingHotel = crudContext.Hotels.Find(hotel.Id);
            if (existingHotel != null)
            {
                existingHotel.Location = hotel.Location;
                existingHotel.Name = hotel.Name;
                crudContext.Hotels.Update(existingHotel);
                crudContext.SaveChanges();
            }

            return hotel;
        }
    }
}
