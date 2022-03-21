using System.Diagnostics.CodeAnalysis;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.Domain.GoalDomain;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace MeFit_BE.Models
{
    public class MeFitDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<WorkoutProgram> WorkoutPrograms { get; set; } 
        public DbSet<Set> Sets { get; set; } 
        public DbSet<SubGoal> SubGoals { get; set; }
        public DbSet<Workout> Workouts { get; set; }


        public MeFitDbContext([NotNullAttribute] DbContextOptions options) : base(options) { }

        public MeFitDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Addresses
            Address address1 = new Address()
            {
                Id = 1,
                Street = "Karl Johans gate",
                PostalCode = 2849,
                PostalPlace = "Oslo",
                Country = "Norway"
            };
            Address address2 = new Address()
            {
                Id = 2,
                Street = "Lilleveien",
                PostalCode = 9376,
                PostalPlace = "Bergen",
                Country = "Norway"
            };
            Address address3 = new Address()
            {
                Id = 3,
                Street = "Storeveien",
                PostalCode = 3689,
                PostalPlace = "Kautokeino",
                Country = "Norway"
            };

            // Users
            User user1 = new User()
            {
                Id = 1,
                Email = "kari.nordmann@gmail.com",
                FirstName = "Kari",
                LastName = "Nordmann",
                IsAdmin = true,
                IsContributor = true
            };
            User user2 = new User()
            {
                Id = 2,
                Email = "ola.hansen@gmail.com",
                FirstName = "Ola",
                LastName = "Hansen",
                IsContributor = true
            };
            User user3 = new User()
            {
                Id = 3,
                Email = "else.berg@gmail.com",
                FirstName = "Else",
                LastName = "Berg",
            };

            //Profiles
            Profile profile1 = new Profile()
            {
                Id = 1,
                Weight = 89,
                Height = 170,
                MedicalConditions = null,
                Disabilities = null,
                UserId = user1.Id,
                AddressId = address1.Id
            };
            Profile profile2 = new Profile()
            {
                Id = 2,
                Weight = 150,
                Height = 145,
                MedicalConditions = null,
                Disabilities = null,
                UserId = user2.Id,
                AddressId = address2.Id
            };
            Profile profile3 = new Profile()
            {
                Id = 3,
                Weight = 78,
                Height = 164,
                MedicalConditions = null,
                Disabilities = "Wheelchair-bound",
                UserId = user3.Id,
                AddressId = address3.Id
            };

            //WorkoutPrograms
            WorkoutProgram workoutProgram1 = new WorkoutProgram()
            {
                Id = 1,
                Name = "Hot and Heavy",
                ContributorId = user1.Id,
                Difficulty = Domain.Difficulty.Beginner,
                Category = Domain.Category.FullBody
            };
            WorkoutProgram workoutProgram2 = new WorkoutProgram()
            {
                Id = 2,
                Name = "The Wellness Yourney",
                ContributorId = user1.Id,
                Difficulty = Domain.Difficulty.Intermediate,
                Category = Domain.Category.Flexibility
            };
            WorkoutProgram workoutProgram3 = new WorkoutProgram()
            {
                Id = 3,
                Name = "The Runner",
                ContributorId = user2.Id,
                Difficulty = Domain.Difficulty.Expert
            };

            //Goals
            Goal goal1 = new Goal()
            {
                Id = 1,
                EndData = new DateTime(2022, 9, 12),
                UserId = user1.Id,
                WorkoutProgramId = workoutProgram1.Id
            };
            Goal goal2 = new Goal()
            {
                Id = 2,
                EndData = new DateTime(2022, 12, 24),
                Achieved = true,
                UserId = user2.Id,
                WorkoutProgramId = workoutProgram2.Id
            };
            Goal goal3 = new Goal()
            {
                Id = 3,
                EndData = new DateTime(2025, 1, 1),
                UserId = user3.Id,
                Achieved = true,
                WorkoutProgramId = workoutProgram3.Id
            };

            //Workouts
            Workout workout1 = new Workout()
            {
                Id = 1,
                Name = "Strengthify",
                ContributorId = user1.Id,
                Category = Domain.Category.Core,
                Difficulty = Domain.Difficulty.Beginner
            };
            Workout workout2 = new Workout()
            {
                Id = 2,
                Name = "Stamina Builder",
                ContributorId = user1.Id,
                Category = Domain.Category.Stamina,
                Difficulty = Domain.Difficulty.Expert
            };
            Workout workout3 = new Workout()
            {
                Id = 3,
                Name = "Fitness",
                ContributorId = user2.Id,
                Category = Domain.Category.FullBody,
                Difficulty = Domain.Difficulty.Intermediate
            };

            //SubGoals
            SubGoal subGoal1 = new SubGoal()
            {
                Id = 1,
                GoalId = goal1.Id,
                Achieved = true,
                WorkoutId = workout1.Id
            };
            SubGoal subGoal2 = new SubGoal()
            {
                Id = 2,
                GoalId = goal2.Id,
                WorkoutId = workout2.Id
            };
            SubGoal subGoal3 = new SubGoal()
            {
                Id = 3,
                Achieved = true,
                GoalId = goal3.Id,
                WorkoutId = workout3.Id
            };
            SubGoal subGoal4 = new SubGoal()
            {
                Id = 4,
                GoalId = goal2.Id,
                WorkoutId = workout1.Id
            };

            //Exercises
            Exercise exercise1 = new Exercise()
            {
                Id = 1,
                Name = "Crunch",
                Description = 
                "Lay on your back with your hands behind your head, and move your upper body up and down.",
                Image = null,
                Video = null,
                ContributorId = user1.Id,
                Category = Domain.Category.Core
            };
            Exercise exercise2 = new Exercise()
            {
                Id = 2,
                Name = "Push-up",
                Description = 
                "Hands on the floor. Straighten out your body and lift yourself down to the floor and back up by bending you arms.",
                Image = null,
                Video = null,
                ContributorId = user1.Id,
                Category = Domain.Category.Arms
            };
            Exercise exercise3 = new Exercise()
            {
                Id = 3,
                Name = "Plank",
                Description = 
                "Lay down on the floor. Then lift and hold yourself up on your elbows and toes. Hold and breath.",
                Image = null,
                Video = null,
                ContributorId = user2.Id,
                Category = Domain.Category.FullBody,
            };
            Exercise exercise4 = new Exercise()
            {
                Id = 4,
                Name = "Jumping Jacks",
                Description = 
                "Jump up and down while opening and closing your legs and lifting your arms over your head.",
                Image = null,
                Video = null,
                ContributorId = user2.Id,
                Category = Domain.Category.Stamina
            };

            //Sets
            Set set1 = new Set()
            {
                Id = 1,
                ExerciseRepetitions = 20,
                WorkoutId = workout1.Id,
                ExerciseId = exercise1.Id,
            };
            Set set2 = new Set()
            {
                Id = 2,
                ExerciseRepetitions = 10,
                WorkoutId = workout2.Id,
                ExerciseId = exercise2.Id
            };
            Set set3 = new Set()
            {
                Id = 3,
                ExerciseRepetitions = 1,
                WorkoutId = workout3.Id,
                ExerciseId = exercise3.Id
            };
            Set set4 = new Set()
            {
                Id = 4,
                ExerciseRepetitions = 30,
                WorkoutId = workout2.Id,
                ExerciseId = exercise4.Id
            };

            // Save user domain tables

            //Save addresses
            modelBuilder.Entity<Address>().HasData(address1);
            modelBuilder.Entity<Address>().HasData(address2);
            modelBuilder.Entity<Address>().HasData(address3);

            //Save profiles
            modelBuilder.Entity<Profile>().HasData(profile1);
            modelBuilder.Entity<Profile>().HasData(profile2);
            modelBuilder.Entity<Profile>().HasData(profile3);

            //Save users
            modelBuilder.Entity<User>().HasData(user1);
            modelBuilder.Entity<User>().HasData(user2);
            modelBuilder.Entity<User>().HasData(user3);
            
            // Save workout domain tables

            //Save WorkoutPrograms
            modelBuilder.Entity<WorkoutProgram>().HasData(workoutProgram1);
            modelBuilder.Entity<WorkoutProgram>().HasData(workoutProgram2);
            modelBuilder.Entity<WorkoutProgram>().HasData(workoutProgram3);

            //Save Workouts
            modelBuilder.Entity<Workout>().HasData(workout1);
            modelBuilder.Entity<Workout>().HasData(workout2);
            modelBuilder.Entity<Workout>().HasData(workout3);
            
            //Save exercises
            modelBuilder.Entity<Exercise>().HasData(exercise1);
            modelBuilder.Entity<Exercise>().HasData(exercise2);
            modelBuilder.Entity<Exercise>().HasData(exercise3);
            modelBuilder.Entity<Exercise>().HasData(exercise4);

            //Save sets
            modelBuilder.Entity<Set>().HasData(set1);
            modelBuilder.Entity<Set>().HasData(set2);
            modelBuilder.Entity<Set>().HasData(set3);
            modelBuilder.Entity<Set>().HasData(set4);

            // Save goal domain tables

            //Save goals
            modelBuilder.Entity<Goal>().HasData(goal1);
            modelBuilder.Entity<Goal>().HasData(goal2);
            modelBuilder.Entity<Goal>().HasData(goal3);

            //Save SubGoals
            modelBuilder.Entity<SubGoal>().HasData(subGoal1);
            modelBuilder.Entity<SubGoal>().HasData(subGoal2);
            modelBuilder.Entity<SubGoal>().HasData(subGoal3);
            modelBuilder.Entity<SubGoal>().HasData(subGoal4);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data source=ND-5CG9030MCG\\SQLEXPRESS; Initial Catalog=MovieInfoAPIDB; Integrated Security=True;");
        }
    }
}
