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
        public DbSet<ContributorRequest> ContributorRequests { get; set; }


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
            User anne = new User()
            {
                Id = 9,
                Email = "anneelarsen98@gmail.com",
                FirstName = "Anne E.",
                LastName = "Larsen",
                IsContributor = true
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
                Difficulty = Domain.Difficulty.BEGINNER,
                Category = Domain.Category.FULL_BODY
            };
            WorkoutProgram workoutProgram2 = new WorkoutProgram()
            {
                Id = 2,
                Name = "The Wellness Yourney",
                ContributorId = user1.Id,
                Difficulty = Domain.Difficulty.INTERMEDIATE,
                Category = Domain.Category.FLEXIBILITY
            };
            WorkoutProgram workoutProgram3 = new WorkoutProgram()
            {
                Id = 3,
                Name = "The Runner",
                ContributorId = user2.Id,
                Difficulty = Domain.Difficulty.EXPERT,
                Category = Domain.Category.STAMINA
            };

            WorkoutProgram niceAndEasy = new WorkoutProgram()
            {
                Id = 4,
                Name = "Nice and Easy",
                ContributorId = anne.Id,
                Difficulty = Domain.Difficulty.BEGINNER,
                Category = Domain.Category.FULL_BODY
            };
            WorkoutProgram compoundCollection = new WorkoutProgram()
            {
                Id = 5,
                Name = "The Compound Collection",
                ContributorId = anne.Id,
                Difficulty = Domain.Difficulty.INTERMEDIATE,
                Category = Domain.Category.FULL_BODY
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
                Category = Domain.Category.CORE,
                Difficulty = Domain.Difficulty.BEGINNER
            };
            Workout workout2 = new Workout()
            {
                Id = 2,
                Name = "Stamina Builder",
                ContributorId = user1.Id,
                Category = Domain.Category.STAMINA,
                Difficulty = Domain.Difficulty.EXPERT
            };
            Workout workout3 = new Workout()
            {
                Id = 3,
                Name = "Fitness",
                ContributorId = user2.Id,
                Category = Domain.Category.FULL_BODY,
                Difficulty = Domain.Difficulty.INTERMEDIATE
            };
            Workout nae = new Workout()
            {
                Id = 4,
                Name = "Machine Trio",
                ContributorId = anne.Id,
                Category = Domain.Category.ARMS,
                Difficulty = Domain.Difficulty.BEGINNER
            };
            Workout tcc1 = new Workout()
            {
                Id = 5,
                Name = "The Compound Collection 1",
                ContributorId = anne.Id,
                Category = Domain.Category.FULL_BODY,
                Difficulty = Domain.Difficulty.INTERMEDIATE
            };
            Workout tcc2 = new Workout()
            {
                Id = 6,
                Name = "The Compound Collection 2",
                ContributorId = anne.Id,
                Category = Domain.Category.ARMS,
                Difficulty = Domain.Difficulty.INTERMEDIATE
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
                Image = "https://us.123rf.com/450wm/lioputra/lioputra2011/lioputra201100006/158485483-man-doing-sit-ups-exercise-abdominals-exercise-flat-vector-illustration-isolated-on-white-background.jpg?ver=6",
                Video = null,
                ContributorId = user1.Id,
                Category = Domain.Category.CORE
            };
            Exercise exercise2 = new Exercise()
            {
                Id = 2,
                Name = "Push-up",
                Description = 
                "Hands on the floor. Straighten out your body and lift yourself down to the floor and back up by bending you arms.",
                Image = null,
                Video = "https://youtu.be/uCNgB_rU3IQ?t=5",
                ContributorId = user1.Id,
                Category = Domain.Category.ARMS
            };
            Exercise exercise3 = new Exercise()
            {
                Id = 3,
                Name = "Plank",
                Description = 
                "Lay down on the floor. Then lift and hold yourself up on your elbows and toes. Hold and breath.",
                Image = null,
                Video = "https://youtu.be/HW4yjoCkbm0?t=5",
                ContributorId = user2.Id,
                Category = Domain.Category.FULL_BODY
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
                Category = Domain.Category.STAMINA
            };
            Exercise machineChest = new Exercise()
            {
                Id = 5,
                Name = "Chest-press Machine",
                Description =
                "Adjust seat and weights to an approperiate level. " +
                "Grab handles, your elbows should be parallell to the floor. " +
                "Push handles away from your chest by extending your elbows all the way. " +
                "Make sure your back remains in contact with the backrest throughout. " +
                "Pull your arms back towards you with controll. Repeat.",
                Image = null, 
                Video = "https://youtu.be/IbeA5ypeMns?t=5",
                ContributorId = anne.Id,
                Category = Domain.Category.ARMS
            };
            Exercise machineShoulder = new Exercise()
            {
                Id = 6,
                Name = "Seated Shoulder-press Machine",
                Description =
                "Adjust seat and weights to an approperiate level. " +
                "Grab handles, your elbows should point to the floor. " +
                "Lift the handles by extending your elbows all the way. " +
                "Make sure your lower back remains in contact with the backrest throughout. " +
                "Lower arms with controll. Repeat.",
                Image = null,
                Video = "https://youtu.be/OD5pz7-703U",
                ContributorId = anne.Id,
                Category = Domain.Category.ARMS
            };
            Exercise machineRowing = new Exercise()
            {
                Id = 7,
                Name = "Rowing Machine",
                Description =
                "Row, row, row your boat.",
                Image = null,
                Video = "https://youtu.be/g2Q-etHs9LI?t=4",
                ContributorId = anne.Id,
                Category = Domain.Category.ARMS
            };
            Exercise compoundDeadlift = new Exercise()
            {
                Id = 8,
                Name = "Deadlift",
                Description = 
                "Stand with your feet shoulder-width apart. " +
                "Grasp the bar with your hands just outside your legs. " +
                "Lift the bar by driving your hips forward, keeping a flat back. " +
                "Lower the bar with controll. Repeat.",
                Image = "https://cdn.mos.cms.futurecdn.net/pcDfKtAmMLgLLbXc8sSAkF-970-80.jpg.webp",
                Video = "https://youtu.be/ABga0-lEY58?t=5",
                ContributorId = anne.Id,
                Category= Domain.Category.LEGS
            };
            Exercise compoundPullup = new Exercise()
            {
                Id = 9,
                Name = "Pull Up",
                Description =
                "Grab onto the bar and hang with your arms fully extended. " +
                "Pull yourself up, with controll, untill your chin is above the bar. " +
                "Try to keep the rest of your body still, be mindfull not to bend your hips or knees. " +
                "Slowly lower yourself down into the starting position, with controll. Repeat.",
                Image = "https://evofitness.no/wp-content/uploads/2019/12/pullupfront.png__666x666_q85_subsampling-2.jpeg",
                Video = null,
                ContributorId = anne.Id,
                Category = Domain.Category.ARMS
            };
            Exercise compoundBenchPress = new Exercise()
            {
                Id = 10,
                Name = "Bench Press",
                Description =
                "Lie down on your back on the bench. " +
                "Your feet should rest flat on the ground. " +
                "Grasp the bar, positioning your hands slightly wider than your shoulders. " +
                "Lift the bar and hold it over your chest. " +
                "Slowly lower the bar towards your chest. " +
                "Push the bar away from your chest, until your arms are fully extended. Repeat",
                Image = "https://image.shutterstock.com/image-illustration/closegrip-barbell-bench-press-3d-260nw-430936051.jpg",
                Video = null,
                ContributorId = anne.Id,
                Category = Domain.Category.ARMS
            };
            Exercise compoundDips = new Exercise()
            {
                Id = 11,
                Name = "Dips",
                Description =
                "Grasp the two bars. " +
                "Extend your arms so that they support your full weight, your legs should be hanging. " +
                "Lower your body down by bending your elbows. " +
                "Throughout the exercise, your elbows should be in line with your wrists. " +
                "Your shoulders should be almost parallell with your elbows before pushing your body up again. Repeat.",
                Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/59/Dipexercise.svg/300px-Dipexercise.svg.png",
                Video = null,
                ContributorId = anne.Id,
                Category = Domain.Category.ARMS
            };

            //Sets
            Set set1 = new Set()
            {
                Id = 1,
                ExerciseRepetitions = 20,
                WorkoutId = workout1.Id,
                ExerciseId = exercise1.Id
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
            Set nae1 = new Set()
            {
                Id = 5,
                ExerciseRepetitions = 15,
                WorkoutId = nae.Id,
                ExerciseId = machineChest.Id
            };
            Set nae2 = new Set()
            {
                Id = 6,
                ExerciseRepetitions = 15,
                WorkoutId = nae.Id,
                ExerciseId = machineShoulder.Id
            };
            Set nae3 = new Set()
            {
                Id = 7,
                ExerciseRepetitions = 15,
                WorkoutId = nae.Id,
                ExerciseId = machineRowing.Id
            };
            Set tcc11 = new Set()
            {
                Id = 8,
                ExerciseRepetitions = 12,
                WorkoutId = tcc1.Id,
                ExerciseId = compoundDeadlift.Id
            };
            Set tcc12 = new Set()
            {
                Id = 9,
                ExerciseRepetitions = 12,
                WorkoutId = tcc1.Id,
                ExerciseId = compoundPullup.Id
            };
            Set tcc21 = new Set()
            {
                Id = 10,
                ExerciseRepetitions = 12,
                WorkoutId = tcc2.Id,
                ExerciseId = compoundBenchPress.Id
            };
            Set tcc22 = new Set()
            {
                Id = 11,
                ExerciseRepetitions = 12,
                WorkoutId = tcc2.Id,
                ExerciseId = compoundDips.Id
            };

            modelBuilder.Entity<Set>()
                    .HasOne(s => s.Workout).WithMany(w => w.Sets)
                    .HasForeignKey(s => s.WorkoutId)
                    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Set>()
                    .HasOne(s => s.Exercise).WithMany(e => e.Sets)
                    .HasForeignKey(s => s.ExerciseId)
                    .OnDelete(DeleteBehavior.NoAction);

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
            modelBuilder.Entity<User>().HasData(anne);

            // Save workout domain tables

            //Save WorkoutPrograms
            modelBuilder.Entity<WorkoutProgram>().HasData(workoutProgram1);
            modelBuilder.Entity<WorkoutProgram>().HasData(workoutProgram2);
            modelBuilder.Entity<WorkoutProgram>().HasData(workoutProgram3);
            modelBuilder.Entity<WorkoutProgram>().HasData(niceAndEasy);
            modelBuilder.Entity<WorkoutProgram>().HasData(compoundCollection);

            //Save Workouts
            modelBuilder.Entity<Workout>().HasData(workout1);
            modelBuilder.Entity<Workout>().HasData(workout2);
            modelBuilder.Entity<Workout>().HasData(workout3);
            modelBuilder.Entity<Workout>().HasData(nae);
            modelBuilder.Entity<Workout>().HasData(tcc1);
            modelBuilder.Entity<Workout>().HasData(tcc2);

            //Save exercises
            modelBuilder.Entity<Exercise>().HasData(exercise1);
            modelBuilder.Entity<Exercise>().HasData(exercise2);
            modelBuilder.Entity<Exercise>().HasData(exercise3);
            modelBuilder.Entity<Exercise>().HasData(exercise4);
            modelBuilder.Entity<Exercise>().HasData(machineChest);
            modelBuilder.Entity<Exercise>().HasData(machineRowing);
            modelBuilder.Entity<Exercise>().HasData(machineShoulder);
            modelBuilder.Entity<Exercise>().HasData(compoundBenchPress);
            modelBuilder.Entity<Exercise>().HasData(compoundDeadlift);
            modelBuilder.Entity<Exercise>().HasData(compoundDips);
            modelBuilder.Entity<Exercise>().HasData(compoundPullup);

            //Save sets
            modelBuilder.Entity<Set>().HasData(set1);
            modelBuilder.Entity<Set>().HasData(set2);
            modelBuilder.Entity<Set>().HasData(set3);
            modelBuilder.Entity<Set>().HasData(set4);
            modelBuilder.Entity<Set>().HasData(nae1);
            modelBuilder.Entity<Set>().HasData(nae2);
            modelBuilder.Entity<Set>().HasData(nae3);
            modelBuilder.Entity<Set>().HasData(tcc11);
            modelBuilder.Entity<Set>().HasData(tcc12);
            modelBuilder.Entity<Set>().HasData(tcc21);
            modelBuilder.Entity<Set>().HasData(tcc22);

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

        
        // Reguired when having more than one migration.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                    //"Data Source= ND-5CG92747KF\\SQLEXPRESS; Initial Catalog= MeFitDB; Integrated Security=True;" // Anne
                    "Data source=ND-5CG9030MCG\\SQLEXPRESS; Initial Catalog=MeFitDB; Integrated Security=True;" // Miriam

                );
        }
        
    }
}
