using Hotelio.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Context
{
    public class CrudContext : IdentityDbContext<User>
    {
        public CrudContext(DbContextOptions<CrudContext> options) : base(options)
        {

        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<AdditionalService> AdditionalServices { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
