using Microsoft.EntityFrameworkCore;
using ParkyApi.Models;

namespace ParkyApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<NationalPark> NationalPark { get; set; }
        public DbSet<Trail> Trail { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
