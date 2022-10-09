using Hotelio.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelio.Context
{
    public class CrudContext : DbContext
    {
        public CrudContext(DbContextOptions<CrudContext> options) : base(options)
        {

        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<AdditionalService> AdditionalServices { get; set; }
    }
}
