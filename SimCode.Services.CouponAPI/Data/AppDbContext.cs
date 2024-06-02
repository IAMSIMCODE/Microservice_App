using Microsoft.EntityFrameworkCore;
using SimCode.Services.EmailApi.Models;

namespace SimCode.Services.EmailApi.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Coupon> Coupons { get; set; }


        //Seed some basic data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                CouponCode = "100FF",
                DiscountAmount = 10,
                MinAmount = 10,
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CouponCode = "200FF",
                DiscountAmount = 20,
                MinAmount = 30,
            });
        }
    }
}
