using Microsoft.EntityFrameworkCore;
using SimCode.Services.ShoppingCartApi.Models;

namespace SimCode.Services.EmailApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) 
    {
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetail> cartDetails { get; set; }
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
