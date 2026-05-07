using Microsoft.EntityFrameworkCore;
using WellsAPI.Entities;

namespace WellsAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Well> Wells { get; set; }
    }
}
