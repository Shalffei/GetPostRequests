using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetPostRequests.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace GetPostRequests.EntityFramework
{
    public class ApplicationDBContextJson : DbContext
    {
        public DbSet<JsonVideocard> Videocards { get; set; }
        public ApplicationDBContextJson()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VideocardsTable;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().HasIndex(x => x.Name).IsUnique();
        }
    }
}
