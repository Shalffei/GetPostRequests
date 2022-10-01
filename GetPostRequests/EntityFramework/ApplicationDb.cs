using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetPostRequests.Models;
using Microsoft.EntityFrameworkCore;



namespace GetPostRequests.EntityFramework
{
    public class ApplicationDb : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public ApplicationDb()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UserTable;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Name).IsUnique();
        }
    }
}
