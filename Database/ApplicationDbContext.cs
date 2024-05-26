using Microsoft.EntityFrameworkCore;
using WebProjektRazor.Models;
using WebProjektRazor.Models.User;

namespace WebProjektRazor.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<OrderService> OrderServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasDiscriminator<UserRole>("Role")
                .HasValue<Client>(UserRole.Client)
                .HasValue<Employee>(UserRole.Employee);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.HistoryProductsOrders)
                .WithOne(op => op.Client)
                .HasForeignKey(op => op.ClientId);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.HistoryServiceOrders)
                .WithOne(os => os.Client)
                .HasForeignKey(os => os.ClientId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.AssignedOrders)
                .WithOne(o => o.Employee)
                .HasForeignKey(o => o.EmployeeId);

            modelBuilder.Entity<OrderProducts>()
                .HasMany(op => op.Products)
                .WithMany(p => p.OrderProducts)
                .UsingEntity(j => j.ToTable("OrderProducts_Products"));

            modelBuilder.Entity<OrderService>()
                .HasMany(os => os.Services)
                .WithMany(s => s.OrderServices)
                .UsingEntity(j => j.ToTable("OrderService_Services"));

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Client)
                .WithMany()
                .HasForeignKey(c => c.ClientId);

        }
    }
}
