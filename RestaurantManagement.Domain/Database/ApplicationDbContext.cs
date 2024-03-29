using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Domain.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<RankHistory> RankHistories { get; set; }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<OrderPizzaa> OrderPizzaas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Pizza>()
                .HasOne(p => p.Image)
                .WithOne(i => i.Pizza)
                .HasForeignKey<Image>(i => i.PizzaId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderPizzaa>()
                .HasOne(op => op.Pizza)
                .WithOne()
                .HasForeignKey<OrderPizzaa>(op => op.PizzaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderPizzaa>()
                .HasOne(op => op.User)
                .WithOne()
                .HasForeignKey<OrderPizzaa>(op => op.UserId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderPizzaa>()
                .HasOne(op => op.Address)
                .WithOne()
                .HasForeignKey<OrderPizzaa>(op => op.AddresId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RankHistory>()
                .HasOne(i => i.User)
                .WithOne()
                .HasForeignKey<RankHistory>(i => i.UserId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RankHistory>()
               .HasOne(i => i.Pizza)
               .WithOne()
               .HasForeignKey<RankHistory>(i => i.PizzaId)
    .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<Address>().HasKey(x => x.Id);
            modelBuilder.Entity<Image>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderPizzaa>().HasKey(x => x.Id);
            modelBuilder.Entity<Pizza>().HasKey(x => x.Id);
            modelBuilder.Entity<RankHistory>().HasKey(x => x.Id);

        }
    }
}
