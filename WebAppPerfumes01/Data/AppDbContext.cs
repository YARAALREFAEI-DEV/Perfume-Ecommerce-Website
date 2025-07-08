using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppPerfumes01.Models;

namespace WebAppPerfumes01.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
             : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<DeliveryArea> DeliveryAreas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<User> users { get; set; }

        /*        protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    base.OnModelCreating(modelBuilder);

                    modelBuilder.Entity<OrderDetail>()
                        .HasOne(o => o.Order)
                        .WithMany(o => o.OrderDetails)
                        .HasForeignKey(o => o.OrderId);

                    modelBuilder.Entity<OrderItem>()
                        .HasOne(oi => oi.Product)
                        .WithMany()
                        .HasForeignKey(oi => oi.ProductId);

                    modelBuilder.Entity<Order>()
                    .Property(o => o.Statu)
                    .HasConversion<string>(); // يخزن enum كنص

                    base.OnModelCreating(modelBuilder);



                }*/


 /*       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.Statu)
                .HasConversion<string>(); // يخزن enum كنص

            base.OnModelCreating(modelBuilder);
        }*/
    }
}
