using System.Diagnostics.CodeAnalysis;
using MeFit_BE.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MeFit_BE.Models
{
    public class MeFitDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public MeFitDbContext([NotNullAttribute] DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data source=ND-5CG92747KF\\SQLEXPRESS; Initial Catalog=MeFitDB; Integrated Security=True;"); // localserver Anne, change
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User user = new User()
            {
                Id = 1,
                Username = "Miriam"
            };

            modelBuilder.Entity<User>().HasData(user);
        }

    }
}
