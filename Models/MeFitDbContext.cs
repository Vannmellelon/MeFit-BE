using System.Diagnostics.CodeAnalysis;
using MeFit_BE.Models.Domain.User;
using MeFit_BE.Models.Domain.Workout;
using Microsoft.EntityFrameworkCore;


namespace MeFit_BE.Models
{
    public class MeFitDbContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<WorkoutProgram> WorkoutPrograms { get; set; } 
        public DbSet<Set> Sets { get; set; } 
        public DbSet<SubGoal> SubGoals { get; set; }
        public DbSet<Workout> Workouts { get; set; }



        public MeFitDbContext([NotNullAttribute] DbContextOptions options) : base(options) { }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data source=ND-5CG92747KF\\SQLEXPRESS; Initial Catalog=MeFitDB; Integrated Security=True;"); // localserver Anne, change
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User user = new User()
            {
                Id = 1,
                Username = "Miriam"
            };

            

            //Profile profile = new Profile
            //{
            //    Id = 1,
            //    Address 
            //};

            modelBuilder.Entity<User>().HasData(user);
        }

    }
}
