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
    [Migration("20220314091010_INIT")]
    partial class INIT
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("MeFit_BE.Models.Domain.UserDomain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Disabilities")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsContributer")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedicalConditions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AddressId = 1,
                            Email = "kari.nordmann@gmail.com",
                            FirstName = "Kari",
                            Height = 170,
                            IsAdmin = true,
                            IsContributer = true,
                            LastName = "Nordmann",
                            Weight = 89
                        },
                        new
                        {
                            Id = 2,
                            AddressId = 2,
                            Email = "ola.hansen@gmail.com",
                            FirstName = "Ola",
                            Height = 145,
                            IsAdmin = false,
                            IsContributer = true,
                            LastName = "Hansen",
                            Weight = 150
                        },
                        new
                        {
                            Id = 3,
                            AddressId = 3,
                            Disabilities = "Wheelchair-bound",
                            Email = "else.berg@gmail.com",
                            FirstName = "Else",
                            Height = 164,
                            IsAdmin = false,
                            IsContributer = false,
                            LastName = "Berg",
                            Weight = 78
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

                    b.Property<int>("SetId")
                        .HasColumnType("int");

                    b.Property<string>("TargetMuscleGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Video")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContributedById");

                    b.HasIndex("SetId");

                    b.ToTable("Exercise");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContributorId = 0,
                            Description = "Lay on your back with your hands behind your head, and move your upper body up and down.",
                            Name = "Crunch",
                            SetId = 1,
                            TargetMuscleGroup = "Stomach"
                        },
                        new
                        {
                            Id = 2,
                            ContributorId = 0,
                            Description = "Hands on the floor. Straighten out your body and lift yourself down to the floor and back up by bending you arms.",
                            Name = "Push-up",
                            SetId = 2,
                            TargetMuscleGroup = "Arms"
                        },
                        new
                        {
                            Id = 3,
                            ContributorId = 0,
                            Description = "Lay down on the floor. Then lift and hold yourself up on your elbows and toes. Hold and breath.",
                            Name = "Plank",
                            SetId = 3,
                            TargetMuscleGroup = "All"
                        },
                        new
                        {
                            Id = 4,
                            ContributorId = 0,
                            Description = "Jump up and down while opening and closing your legs and lifting your arms over your head.",
                            Name = "Jumping Jacks",
                            SetId = 4,
                            TargetMuscleGroup = "Stamina"
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Goal", b =>
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

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Goal");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Achieved = false,
                            EndData = new DateTime(2022, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Achieved = true,
                            EndData = new DateTime(2022, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            Achieved = true,
                            EndData = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = 3
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Set", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ExerciseRepetitions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkoutId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutId");

                    b.ToTable("Set");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ExerciseRepetitions = "20",
                            WorkoutId = 1
                        },
                        new
                        {
                            Id = 2,
                            ExerciseRepetitions = "10",
                            WorkoutId = 2
                        },
                        new
                        {
                            Id = 3,
                            ExerciseRepetitions = "1",
                            WorkoutId = 3
                        },
                        new
                        {
                            Id = 4,
                            ExerciseRepetitions = "30",
                            WorkoutId = 2
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.SubGoal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Achieved")
                        .HasColumnType("bit");

                    b.Property<int>("WorkoutProgramId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutProgramId");

                    b.ToTable("SubGoal");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Achieved = false,
                            WorkoutProgramId = 1
                        },
                        new
                        {
                            Id = 2,
                            Achieved = false,
                            WorkoutProgramId = 2
                        },
                        new
                        {
                            Id = 3,
                            Achieved = true,
                            WorkoutProgramId = 3
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Workout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<int?>("ContributedById")
                        .HasColumnType("int");

                    b.Property<int>("ContributorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubGoalId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContributedById");

                    b.HasIndex("SubGoalId");

                    b.ToTable("Workout");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Complete = false,
                            ContributorId = 0,
                            Name = "Strengthify",
                            SubGoalId = 1,
                            Type = "Strength"
                        },
                        new
                        {
                            Id = 2,
                            Complete = false,
                            ContributorId = 0,
                            Name = "Stamina Builder",
                            SubGoalId = 2,
                            Type = "Stamina"
                        },
                        new
                        {
                            Id = 3,
                            Complete = true,
                            ContributorId = 0,
                            Name = "Fitness",
                            SubGoalId = 3,
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

                    b.Property<int>("GoalId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContributedById");

                    b.HasIndex("GoalId");

                    b.ToTable("WorkoutProgram");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Upper-body Strength",
                            ContributorId = 0,
                            GoalId = 1,
                            Name = "Hot and Heavy"
                        },
                        new
                        {
                            Id = 2,
                            Category = "Fitness",
                            ContributorId = 0,
                            GoalId = 2,
                            Name = "The Wellness Yourney"
                        },
                        new
                        {
                            Id = 3,
                            Category = "Stamina",
                            ContributorId = 0,
                            GoalId = 3,
                            Name = "The Runner"
                        });
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.UserDomain.User", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Exercise", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.User", "ContributedBy")
                        .WithMany()
                        .HasForeignKey("ContributedById");

                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.Set", "Set")
                        .WithMany()
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContributedBy");

                    b.Navigation("Set");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Goal", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.User", "User")
                        .WithMany("Goals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Set", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.Workout", "Workout")
                        .WithMany("Sets")
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.SubGoal", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.WorkoutProgram", "WorkoutProgram")
                        .WithMany("SubGoals")
                        .HasForeignKey("WorkoutProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkoutProgram");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Workout", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.User", "ContributedBy")
                        .WithMany()
                        .HasForeignKey("ContributedById");

                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.SubGoal", "SubGoal")
                        .WithMany()
                        .HasForeignKey("SubGoalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContributedBy");

                    b.Navigation("SubGoal");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.WorkoutProgram", b =>
                {
                    b.HasOne("MeFit_BE.Models.Domain.UserDomain.User", "ContributedBy")
                        .WithMany()
                        .HasForeignKey("ContributedById");

                    b.HasOne("MeFit_BE.Models.Domain.WorkoutDomain.Goal", "Goal")
                        .WithMany("WorkoutProgram")
                        .HasForeignKey("GoalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContributedBy");

                    b.Navigation("Goal");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.UserDomain.User", b =>
                {
                    b.Navigation("Goals");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Goal", b =>
                {
                    b.Navigation("WorkoutProgram");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.Workout", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("MeFit_BE.Models.Domain.WorkoutDomain.WorkoutProgram", b =>
                {
                    b.Navigation("SubGoals");
                });
#pragma warning restore 612, 618
        }
    }
}
