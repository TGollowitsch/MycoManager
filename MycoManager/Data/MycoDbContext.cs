using Microsoft.EntityFrameworkCore;
using MycoManager.Models;

namespace MycoManager.Data
{
    public class MycoDbContext : DbContext
    {
        public DbSet<EdgeSensor> EdgeSensors { get; set; }
        public DbSet<MycoStrain> MycoStrains { get; set; }

        public MycoDbContext(DbContextOptions<MycoDbContext> options) : base(options) { }
    }
}
