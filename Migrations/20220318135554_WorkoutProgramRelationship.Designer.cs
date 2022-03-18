﻿// <auto-generated />
using System;
using MeFit_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MeFit_BE.Migrations
{
    [DbContext(typeof(MeFitDbContext))]
    [Migration("20220318135554_WorkoutProgramRelationship")]
    partial class WorkoutProgramRelationship
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MeFit_BE.Models.Domain.GoalDomain.Goal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Achieved")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EndData")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WorkoutProgramId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkoutProgramId");

                    b.ToTable("Goal");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Achieved = false,
                            EndData = new DateTime(2022, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 1,
                            WorkoutProgramId = 1
                        },
                        new
                        {
                            Id = 2,
                            Achieved = true,
                            EndData = new DateTime(2022, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 2,
                            WorkoutProgramId = 2
                        },
                        new
                        {
                            Id = 3,
                            Achieved = true,
                            EndData = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 3,
                            WorkoutProgramId = 3
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.GoalDomain.SubGoal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Achieved")
                        .HasColumnType("bit");

                    b.Property<int>("GoalId")
                        .HasColumnType("int");

                    b.Property<int>("WorkoutId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GoalId");

                    b.HasIndex("WorkoutId");

                    b.ToTable("SubGoal");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Achieved = true,
                            GoalId = 1,
                            WorkoutId = 1
                        },
                        new
                        {
                            Id = 2,
                            Achieved = false,
                            GoalId = 2,
                            WorkoutId = 2
                        },
                        new
                        {
                            Id = 3,
                            Achieved = true,
                            GoalId = 3,
                            WorkoutId = 3
                        },
                        new
                        {
                            Id = 4,
                            Achieved = false,
                            GoalId = 2,
                            WorkoutId = 1
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.UserDomain.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostalCode")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.Property<string>("PostalPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Address");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Country = "Norway",
                            PostalCode = 2849,
                            PostalPlace = "Oslo",
                            Street = "Karl Johans gate"
                        },
                        new
                        {
                            Id = 2,
                            Country = "Norway",
                            PostalCode = 9376,
                            PostalPlace = "Bergen",
                            Street = "Lilleveien"
                        },
                        new
                        {
                            Id = 3,
                            Country = "Norway",
                            PostalCode = 3689,
                            PostalPlace = "Kautokeino",
                            Street = "Storeveien"
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.UserDomain.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Disabilities")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("MedicalConditions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Profile");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AddressId = 1,
                            Height = 170,
                            UserId = 1,
                            Weight = 89
                        },
                        new
                        {
                            Id = 2,
                            AddressId = 2,
                            Height = 145,
                            UserId = 2,
                            Weight = 150
                        },
                        new
                        {
                            Id = 3,
                            AddressId = 3,
                            Disabilities = "Wheelchair-bound",
                            Height = 164,
                            UserId = 3,
                            Weight = 78
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.UserDomain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsContributor")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "kari.nordmann@gmail.com",
                            FirstName = "Kari",
                            IsAdmin = true,
                            IsContributor = true,
                            LastName = "Nordmann"
                        },
                        new
                        {
                            Id = 2,
                            Email = "ola.hansen@gmail.com",
                            FirstName = "Ola",
                            IsAdmin = false,
                            IsContributor = true,
                            LastName = "Hansen"
                        },
                        new
                        {
                            Id = 3,
                            Email = "else.berg@gmail.com",
                            FirstName = "Else",
                            IsAdmin = false,
                            IsContributor = false,
                            LastName = "Berg"
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContributedById")
                        .HasColumnType("int");

                    b.Property<int>("ContributorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TargetMuscleGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Video")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContributedById");

                    b.ToTable("Exercise");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContributorId = 0,
                            Description = "Lay on your back with your hands behind your head, and move your upper body up and down.",
                            Name = "Crunch",
                            TargetMuscleGroup = "Stomach"
                        },
                        new
                        {
                            Id = 2,
                            ContributorId = 0,
                            Description = "Hands on the floor. Straighten out your body and lift yourself down to the floor and back up by bending you arms.",
                            Name = "Push-up",
                            TargetMuscleGroup = "Arms"
                        },
                        new
                        {
                            Id = 3,
                            ContributorId = 0,
                            Description = "Lay down on the floor. Then lift and hold yourself up on your elbows and toes. Hold and breath.",
                            Name = "Plank",
                            TargetMuscleGroup = "All"
                        },
                        new
                        {
                            Id = 4,
                            ContributorId = 0,
                            Description = "Jump up and down while opening and closing your legs and lifting your arms over your head.",
                            Name = "Jumping Jacks",
                            TargetMuscleGroup = "Stamina"
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Set", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseRepetitions")
                        .HasColumnType("int");

                    b.Property<int>("WorkoutId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("WorkoutId");

                    b.ToTable("Set");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseRepetitions = 20,
                            WorkoutId = 1
                        },
                        new
                        {
                            Id = 2,
                            ExerciseId = 2,
                            ExerciseRepetitions = 10,
                            WorkoutId = 2
                        },
                        new
                        {
                            Id = 3,
                            ExerciseId = 3,
                            ExerciseRepetitions = 1,
                            WorkoutId = 3
                        },
                        new
                        {
                            Id = 4,
                            ExerciseId = 4,
                            ExerciseRepetitions = 30,
                            WorkoutId = 2
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Workout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContributedById")
                        .HasColumnType("int");

                    b.Property<int>("ContributorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContributedById");

                    b.ToTable("Workout");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContributorId = 0,
                            Name = "Strengthify",
                            Type = "Strength"
                        },
                        new
                        {
                            Id = 2,
                            ContributorId = 0,
                            Name = "Stamina Builder",
                            Type = "Stamina"
                        },
                        new
                        {
                            Id = 3,
                            ContributorId = 0,
                            Name = "Fitness",
                            Type = "Fitness"
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.WorkoutProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ContributedById")
                        .HasColumnType("int");

                    b.Property<int>("ContributorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContributedById");

                    b.ToTable("WorkoutProgram");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Upper-body Strength",
                            ContributorId = 0,
                            Name = "Hot and Heavy"
                        },
                        new
                        {
                            Id = 2,
                            Category = "Fitness",
                            ContributorId = 0,
                            Name = "The Wellness Yourney"
                        },
                        new
                        {
                            Id = 3,
                            Category = "Stamina",
                            ContributorId = 0,
                            Name = "The Runner"
                        });
                });

            modelBuilder.Entity("WorkoutWorkoutProgram", b =>
                {
                    b.Property<int>("WorkoutProgramsId")
                        .HasColumnType("int");

                    b.Property<int>("WorkoutsId")
                        .HasColumnType("int");

                    b.HasKey("WorkoutProgramsId", "WorkoutsId");

                    b.HasIndex("WorkoutsId");

                    b.ToTable("WorkoutWorkoutProgram");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.GoalDomain.Goal", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.User", "User")
                        .WithMany("Goals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.WorkoutProgram", "WorkoutProgram")
                        .WithMany()
                        .HasForeignKey("WorkoutProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("WorkoutProgram");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.GoalDomain.SubGoal", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.GoalDomain.Goal", "Goal")
                        .WithMany("SubGoals")
                        .HasForeignKey("GoalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.Workout", "Workout")
                        .WithMany()
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Goal");

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.UserDomain.Profile", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Exercise", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.User", "ContributedBy")
                        .WithMany()
                        .HasForeignKey("ContributedById");

                    b.Navigation("ContributedBy");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Set", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.Exercise", "Exercise")
                        .WithMany("Sets")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.Workout", "Workout")
                        .WithMany("Sets")
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Workout", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.User", "ContributedBy")
                        .WithMany()
                        .HasForeignKey("ContributedById");

                    b.Navigation("ContributedBy");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.WorkoutProgram", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.User", "ContributedBy")
                        .WithMany()
                        .HasForeignKey("ContributedById");

                    b.Navigation("ContributedBy");
                });

            modelBuilder.Entity("WorkoutWorkoutProgram", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.WorkoutProgram", null)
                        .WithMany()
                        .HasForeignKey("WorkoutProgramsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.Workout", null)
                        .WithMany()
                        .HasForeignKey("WorkoutsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.GoalDomain.Goal", b =>
                {
                    b.Navigation("SubGoals");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.UserDomain.User", b =>
                {
                    b.Navigation("Goals");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Exercise", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Workout", b =>
                {
                    b.Navigation("Sets");
                });
#pragma warning restore 612, 618
        }
    }
}
