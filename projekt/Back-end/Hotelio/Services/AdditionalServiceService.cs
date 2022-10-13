using Hotelio.Context;
using Hotelio.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Services
{
    public class AdditionalServiceService
    {
        private CrudContext crudContext;
        public AdditionalServiceService(CrudContext crudContext)
        {
            this.crudContext = crudContext;
        }

        public AdditionalService GetAservice(Guid id)
        {
            return crudContext.AdditionalServices
                .Include(x => x.Room)
                .Include(x => x.Room.Hotel)
                .SingleOrDefault(x => x.Id == id);
        }

        public List<AdditionalService> GetAServices()
        {
            return crudContext.AdditionalServices
                .Include(x => x.Room)
                .Include(x => x.Room.Hotel)
                .ToList();
        }
        
        public void DeleteAservice(AdditionalService additionalService)
        {
            crudContext.AdditionalServices.Remove(additionalService);
            crudContext.SaveChanges();
        }

        public AdditionalService AddAservice(Guid RoomId, AdditionalService additionalService)
        {
            var room = crudContext.Rooms.Find(RoomId);
            if (room == null) return null;

            additionalService.Room = room;
            additionalService.Id = Guid.NewGuid();
            crudContext.AdditionalServices.Add(additionalService);
            crudContext.SaveChanges();
            return additionalService;
        }

        public AdditionalService EditAservice(AdditionalService additionalService)
        {
            var existingService = crudContext.AdditionalServices.Find(additionalService.Id);
            if (existingService != null)
            {
                existingService.Name = additionalService.Name;
                existingService.Price = additionalService.Price;
                crudContext.AdditionalServices.Update(existingService);
                crudContext.SaveChanges();
            }

            return additionalService;
        }
    }
}
