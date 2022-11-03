using Hotelio.Auth;
using Hotelio.Data;
using Microsoft.AspNetCore.Identity;

namespace Hotelio.Context
{
    public class DbSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddOwnerUser();
        }

        private async Task AddOwnerUser()
        {
            var ownerUser = new User()
            {
                Email = "owner@owner.lt",
            };

            var existingUser = await _userManager.FindByEmailAsync(ownerUser.Email);
            if (existingUser == null)
            {
                var createdAdminUser = await _userManager.CreateAsync(ownerUser, "owner");
                if (createdAdminUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(ownerUser, HotelRoles.Owner);
                }
            }
        }

        private async Task AddDefaultRoles()
        {
            foreach (var role in HotelRoles.All)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        
    }
}
