using System.Security.Claims;
using Hotelio.Context;
using Hotelio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Services
{
    public class AdditionalServiceService
    {
        private CrudContext crudContext;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public AdditionalServiceService(CrudContext crudContext, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            this.crudContext = crudContext;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        public AdditionalService GetAservice(Guid id)
        {
            return crudContext.AdditionalServices
                .Include(x => x.Room)
                .ThenInclude(x => x.Hotel)
                .SingleOrDefault(x => x.Id == id);
        }

        public List<AdditionalService> GetAServices()
        {
            return crudContext.AdditionalServices
                .Include(x => x.Room)
                .ThenInclude(x => x.Hotel)
                .ToList();
        }
        
        public void DeleteAservice(AdditionalService additionalService)
        {
            crudContext.AdditionalServices.Remove(additionalService);
            crudContext.SaveChanges();
        }

        public AdditionalService AddAservice(string clientId, Guid RoomId, AdditionalService additionalService)
        {
            var room = crudContext.Rooms
                .Include(x => x.Hotel)
                .SingleOrDefault(x => x.Id == RoomId);

            if (room == null) return null;

            additionalService.Room = room;
            additionalService.Id = Guid.NewGuid();
            additionalService.UserId = clientId;

            crudContext.AdditionalServices.Add(additionalService);
            crudContext.SaveChanges();
            return additionalService;
        }

        public async Task<bool> EditAservice(ClaimsPrincipal user, AdditionalService additionalService)
        {
            var existingService = crudContext.AdditionalServices.Find(additionalService.Id);
            if(existingService == null) return false;

            var authResult = await _authorizationService.AuthorizeAsync(user, existingService, "RequestResourceOwner");
            if (!authResult.Succeeded)
            {
                return false;
            }

            if (existingService != null)
            {
                existingService.Name = additionalService.Name;
                existingService.Price = additionalService.Price;
                crudContext.AdditionalServices.Update(existingService);
                crudContext.SaveChanges();
            }

            return true;
        }
    }
}
